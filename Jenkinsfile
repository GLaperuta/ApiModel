pipeline {
    agent any

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        IMAGE_NAME = "myapi"
        POSTGRES_HOST = 'localhost'
        POSTGRES_PORT = '5432'
        POSTGRES_DB = 'myapi'
        REDIS_HOST = 'localhost'
        REDIS_PORT = '6379'
  }
    }

    stages {

        stage('Restore') {
            steps {
                sh 'dotnet restore MyApi.slnx'
            }
        }

        stages {
            stage('Build') {
                steps {
                    withCredentials([
                    string(credentialsId: 'postgres-user', variable: 'POSTGRES_USER'),
                    string(credentialsId: 'postgres-password', variable: 'POSTGRES_PASSWORD')
                    ]) {
                        sh 'dotnet build MyApi.slnx'
                    }
                }
            }
        }

        stage('Tests') {
            steps {
                sh 'dotnet test MyApi.slnx --no-build'
            }
        }

        stage('SonarQube') {
            steps {
                withSonarQubeEnv('SonarQube') {
                    sh '''
                    dotnet-sonarscanner begin /k:"myapi" /d:sonar.host.url="http://sonarqube:9000"
                    dotnet build MyApi.slnx
                    dotnet-sonarscanner end
                    '''
                }
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t myapi:latest -f docker/Dockerfile .'
            }
        }

        stage('Deploy K8s') {
            steps {
                sh 'kubectl apply -f k8s/'
            }
        }
    }
