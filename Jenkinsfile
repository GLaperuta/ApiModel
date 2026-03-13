pipeline {
    agent any

    stages {
        stage('Info') {
            steps {
                sh '''
                echo "Diretório atual:"
                pwd
                echo "Listando arquivos:"
                ls -la
                echo "Versão do dotnet:"
                dotnet --info
                '''
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore MyApi.slnx'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build MyApi.slnx --no-restore'
            }
        }

        stage('Unit Tests') {
            steps {
                sh 'dotnet test tests/MyApi.UnitTests/MyApi.UnitTests.csproj --no-build'
            }
        }
    }
}
