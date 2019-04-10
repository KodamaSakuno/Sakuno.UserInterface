# Sakuno.UserInterface

[![Build status](https://img.shields.io/appveyor/ci/KodamaSakuno/Sakuno-UserInterface/master.svg?style=flat-square)](https://ci.appveyor.com/project/KodamaSakuno/Sakuno-UserInterface)
[![NuGet](https://img.shields.io/nuget/v/Sakuno.UserInterface.svg)](https://www.nuget.org/packages/Sakuno.UserInterface)
[![NuGet](https://img.shields.io/nuget/dt/Sakuno.UserInterface.svg)](https://www.nuget.org/packages/Sakuno.UserInterface)
[![License](https://img.shields.io/github/license/KodamaSakuno/Sakuno.UserInterface.svg?style=flat-square)](./LICENSE.md)

My framework for WPF apps.

## Quick Start

1. Install package via NuGet Package Manager.

```powershell
PM> Install-Package Sakuno.UserInterface
```

2. Change your App class from Application to [Themed App](./src/Sakuno.UserInterface/ThemedApp.cs).

  * You can change theme and accent in constructor.

3. Change your window class(es) from Window to [ModernWindow](./src/Sakuno.UserInterface/Controls/ModernWindow.cs).

4. Compile and run your program.

## Special Features

### RelativePanel

![RelativePanel Sample 1](./docs/images/2019041002.png)

Layout Source: https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.relativepanel#remarks

![RelativePanel Sample 3](./docs/images/2019041003.png)

Layout Source: https://asp-net-example.blogspot.com/2017/01/uwp-understanding-relativepanel.html

### BBCodeBlock

![BBCodeBlock](./docs/images/2019041001.png)

## Other Screenshots

![Common controls](./docs/images/2019040501.png)

![TabControl](./docs/images/2019040502.png)

## Licence

[MIT License (MIT)](./LICENSE.md)
