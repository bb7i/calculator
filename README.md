# 🧮 WinCalc — Калькулятор на C# (Windows Forms)

Простое оконное приложение-калькулятор с цифровой клавиатурой, экраном и панелью истории вычислений.

![C#](https://img.shields.io/badge/C%23-.NET%208-purple)
![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![UI](https://img.shields.io/badge/UI-Windows%20Forms-lightgrey)

---

## 📋 Возможности

- ✅ Базовые арифметические операции: `+`, `-`, `*`, `/`
- ✅ Квадратный корень `√`
- ✅ Смена знака `±` и удаление последнего символа `←`
- ✅ Полная очистка `C`
- ✅ **История вычислений** с возможностью очистки
- ✅ Обработка ошибок (деление на ноль, корень из отрицательного числа)
- ✅ Ввод дробных чисел через точку

---

## 🚀 Запуск

### Требования

- **.NET 8 SDK** (или новее) — [скачать](https://dotnet.microsoft.com/download)
- **Windows** (Windows Forms работает только на Windows)
- **VS Code** или **Visual Studio** (по желанию)

### Установка и запуск

1. Клонируйте репозиторий или создайте проект:
   ```cmd
   dotnet new winforms -n WinCalc
   cd WinCalc
