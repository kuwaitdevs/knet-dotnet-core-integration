# Knet Dotnet Core Integration

.NET Core RAW API Integration of Knet payment gateway.  
This is an unofficial port of the official PHP example provided by Knet.

## Why use RAW integration instead of iPayPipe (DLL)?

- iPayPipe requires [IKVM](https://www.ikvm.net/#Introduction) to work which is not ideal, especially that the IKVM project is not in active development.
- The IKVM version provided by Knet is not compatible with .NET Core.
- This approach reduces dependencies and produces a lighter application footprint.
- The published app is compatible with all platforms.

## Disclaimer

- This is NOT an official port, use it at your own risk. No warranty is provided.
- This is a proof of concept and should not be used in a production environment.
- Please review Knet official documentation first before implementing.

## Development Requirements

- OS: Mac, Windows or Linux
- Visual Studio Code / Visual Studio (for debugging)
- .NET Core 5 SDK (for development) and .NET Core Hosting bundle (for publishing) https://dotnet.microsoft.com/download/dotnet/5.0

## Building and Publishing

- Run `dotnet restore`
- Build with `dotnet build`
- Run locally with `dotnet run`
- Publish with `dotnet publish`

## Before You Start

To test properly, please follow these steps:

- Contact Knet to provide you with Sandbox API details.
- Plug in your KNET details inside the appsettings.json, you will need:
  - Knet Sandbox Tranportal Id
  - Knet Sandbox Tranportal Password
  - Knet Sandbox Payment Api Url (kpaytest)
  - Knet Sandbox Terminal Resource Key
- Test using the checkout form.
- Once everything is OK, contact Knet to provide you with the production API details. (payment flow verification might be required)
- Implement the payment flow in your real project with the necessary adjustments such as:
  - Knet production API endpoint (kpay instead of kpaytest)
  - Proper Response and Error webhooks
  - Production Tranportal details
  - Proper Track ID generation
  - Database Integration
  - Logging
  - Additional Security
