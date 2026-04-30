# UI Test Automation Framework

A demo project showcasing UI test automation for Google Sheets filter functionality.

## Tech Stack

- **.NET 10** (C#)
- **NUnit** - test framework
- **Playwright** - browser automation
- **Custom HTML Report Generator** - test reporting

## Quick Start

### Running Tests
Simply double-click `run.bat`  

Requires:  
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download)
- Windows
- Chrome browser


### Sample Reports

Check out example reports in the root directory:
- `failed-test-example.html` - see how failures are displayed with detailed logs
- `passed-test-example.html` - successful test execution example

> **Tip:** In the report, click on test or step names to expand details.  
> By default, passed tests are hidden - click "Show All Tests" to see everything.

## Project Architecture

```
GoogleDocs/
├── Core/           # Universal UI test automation core
├── Entities/       # Strongly-typed domain entities
├── Forms/          # Page Object pattern implementation
├── Tests/          # Test classes
└── HtmlReport/     # Report generator
```

### Core
The heart of the framework - a universal, reusable core that can be used for automating any website, not just Google Sheets.  
Contains:
- Browser wrappers (Playwright abstraction)
- Element interactions with automatic logging
- Wait utilities and conditions
- Assertion helpers with soft assert support
- Configuration management
- Localization support

### Entities
Strongly-typed business entities representing domain objects (spreadsheets, cells, etc.).  
Keeps test data clean and type-safe.

### Forms
Page Object pattern implementation:
- `Components/` - reusable UI components (Header, Footer, TopMenu)
- `Dialogs/` - modal dialog handlers
- Page classes encapsulating page-specific interactions
- HotKeys abstraction for cross-OS compatibility  

### Tests
NUnit test classes. Each test is self-contained and can run independently or in parallel.  

### HtmlReport
Custom HTML report generator that creates detailed, interactive test reports from NUnit's TRX output.

## Features

### Multi-Browser & Cross-Platform Support
Configured for different browsers and operating systems via `appsettings.json`:
```json
"Browser": {
  "Type": "Chrome",  // Chrome, Firefox
  "OS": "Windows"    // Windows, Mac
}
```
> Currently fully implemented for Windows + Chrome. Other combinations are scaffolded.

### Localization Testing
Test your app in different languages. Configure locale in `appsettings.json`:
```json
"Localization": {
  "Locale": "fr"  // en, fr, etc.
}
```
Localization files (`localization-en.json`, `localization-fr.json`) contain UI element texts. The framework automatically uses correct translations for element selectors.

### Soft Assertions
Don't fail on first error - collect all issues in a single run:
```json
"Assert": {
  "SoftAssertEnabled": true
}
```
All failures are accumulated and reported at the end of the test.

### Comprehensive Logging
Every action is automatically logged:
- Console output during execution
- Detailed logs in HTML reports
- Click, type, wait operations - all tracked
- Screenshots on failure

### Test Steps
Organize tests into logical steps with Step():
```csharp
    [Test]
    public void FilterTest1()
    {
        Step(CreateFilter);
        Step(SetCellValues);
        Step(FilterValuesAndAssert);
    }
```
Steps appear in reports with clear hierarchy and logs.

### Headless Mode
Run tests without visible browser window if necessary:
```json
"Browser": {
  "Headless": true,
  "HeadlessWindowWidth": 2000,
  "HeadlessWindowHeight": 2000
}
```

### Test Tracking Integration
Link tests to your test management and bug tracking systems:
```json
"TestTracking": {
  "TestCaseUrlTemplate": "https://your-tracker.com/testcase/{0}",
  "BugUrlTemplate": "https://your-tracker.com/bug/{0}"
}
```
Test IDs are extracted from method names (e.g., `Test123` → links to testcase/123).

### Parallel Execution
Tests are designed for parallel execution. NUnit handles parallelization automatically.

### Auto-Generated Reports
When running via `run.bat`:
1. Tests execute
2. TRX results are generated
3. HTML report is created automatically
4. Report opens in browser

## Configuration

All settings are in `Tests/appsettings.json`. Key sections:

| Section | Purpose |
|---------|---------|
| `Browser` | Browser type, headless mode, viewport size |
| `Localization` | UI language settings |
| `Assert` | Soft assert behavior |
| `Wait` | Timeouts and polling intervals |
| `Log` | Logging verbosity and options |
| `TestTracking` | External system integration |