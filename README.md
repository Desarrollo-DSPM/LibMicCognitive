# LibMicCognitive

__LibMicCognitive__ Es una libreria de reconocimiento contínuo de voz compilada 
a codigo nativo, usando los servicios de [Microsoft Azure Cognitive Services](https://azure.microsoft.com/es-mx/services/cognitive-services/) y la tecnología [Native AOT](https://docs.microsoft.com/en-us/dotnet/core/deploying/native-aot) de .NET 6.0.

__Su uso fue diseñado principalmente para funcionar con el framework multiplataforma de Google
[Flutter](https://flutter.dev/).__

## Para compilar

Se debe ejecutar el siguiente comando dentro de la carpeta con las propiedades de proyecto (LibMicCognitive/LibMicCognitive).
```batch
dotnet publish /p:NativeLib=Shared /p:SelfContained=true -c debug -r win-x64
```
