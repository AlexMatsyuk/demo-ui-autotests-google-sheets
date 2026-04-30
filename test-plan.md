# Test Plan: Google Sheets Filtering Functionality

---

## 1. Smoke Tests (Critical Path)

Core functionality that must work for the feature to be usable at all.

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SM-01 | Filter by Values | Open filter dropdown, uncheck some values, click OK | Only rows with selected values are displayed |
| SM-02 | Filter by Condition | Open filter dropdown, select "Filter by condition", choose condition (e.g., "Text contains"), enter value, click OK | Only rows matching the condition are displayed |

---

## 2. Sanity Tests (Build Verification)

Quick validation that the feature works correctly after changes.

### 2.1 Filter by Values

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-01 | Filter by Multiple Values | Select several values in filter dropdown | Only rows containing any of selected values are shown |
| SN-02 | Select All Button | Click "Select all" in filter dropdown | All values become checked |
| SN-03 | Clear Button | Click "Clear" in filter dropdown | All values become unchecked |
| SN-04 | Cancel Button | Make filter changes, click Cancel | Filter not applied, original data displayed |
| SN-05 | Search Within Filter | Enter text in search field inside filter dropdown | Only matching values shown in filter list |

### 2.2 Filter by Multiple Columns

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-06 | Filter Two Columns | Apply filter on column A, then apply filter on column B | Rows match both filter conditions (AND logic) |
| SN-07 | Filter Three+ Columns | Apply filters on 3 or more columns | All filter conditions combined correctly |
| SN-08 | Mixed Filter Types on Multiple Columns | Apply filter by color on column A, filter by condition on column B, filter by exact value on column C | All three different filter types work together correctly |

### 2.3 Filter by Color

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-09 | Filter by Fill Color | Select "Filter by color" > "Fill color" > specific color | Only rows with selected fill color shown |
| SN-10 | Filter by Text Color | Select "Filter by color" > "Text color" > specific color | Only rows with selected text color shown |

### 2.4 Filter by Condition - All Conditions

#### Text Conditions

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-11 | Text Contains | Condition: "Text contains" with value | Rows where cell contains specified text |
| SN-12 | Text Does Not Contain | Condition: "Text does not contain" with value | Rows where cell doesn't contain specified text |
| SN-13 | Text Starts With | Condition: "Text starts with" with value | Rows where cell starts with specified text |
| SN-14 | Text Ends With | Condition: "Text ends with" with value | Rows where cell ends with specified text |
| SN-15 | Text Is Exactly | Condition: "Text is exactly" with value | Rows where cell exactly matches specified text |

#### Numeric Conditions

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-16 | Greater Than | Condition: "Greater than" with number | Rows where cell value > specified number |
| SN-17 | Greater Than or Equal To | Condition: "Greater than or equal to" with number | Rows where cell value >= specified number |
| SN-18 | Less Than | Condition: "Less than" with number | Rows where cell value < specified number |
| SN-19 | Less Than or Equal To | Condition: "Less than or equal to" with number | Rows where cell value <= specified number |
| SN-20 | Is Equal To | Condition: "Is equal to" with number | Rows where cell value = specified number |
| SN-21 | Is Not Equal To | Condition: "Is not equal to" with number | Rows where cell value != specified number |
| SN-22 | Is Between | Condition: "Is between" with two numbers | Rows where cell value is in specified range |
| SN-23 | Is Not Between | Condition: "Is not between" with two numbers | Rows where cell value is outside specified range |

#### Date Conditions

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-24 | Date Is | Condition: "Date is" with specific date | Rows where cell date equals specified date |
| SN-25 | Date Is Before | Condition: "Date is before" with date | Rows where cell date is before specified date |
| SN-26 | Date Is After | Condition: "Date is after" with date | Rows where cell date is after specified date |
| SN-27 | Date Is Not Between | Condition: "Date is not between" with date range | Rows where date is outside specified range |

#### Empty/Not Empty Conditions

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-28 | Is Empty | Condition: "Is empty" | Only rows with empty cells in column |
| SN-29 | Is Not Empty | Condition: "Is not empty" | Only rows with non-empty cells in column |

#### Custom Formula

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-30 | Custom Formula Valid | Condition: "Custom formula is" with valid formula | Rows matching custom formula condition |

### 2.5 Filter Scope (Single/Multiple Sheets)

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-31 | Cannot Add Second Filter on Same Sheet | Try to create a new filter when filter already exists on the sheet | Operation blocked, error message or filter menu disabled |
| SN-32 | Filters on Different Sheets | Create filter on Sheet1, then create filter on Sheet2 | Both filters work independently on their respective sheets |

### 2.6 Filter Range Behavior

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-33 | Filter Applies to Selected Range Only | Select range A1:C10, create filter, add data in D1:D10 | Filter applies only to A1:C10, data in column D is not filtered |
| SN-34 | Filter Range Expands on New Rows | Create filter on A1:C5, enter values in A6, A7, A8 sequentially | Filter range automatically expands to include new rows |
| SN-35 | Filter Range Does Not Expand to Adjacent Columns | Create filter on A1:B10, add data in C1:C10 | Filter range stays A1:B10, column C not included automatically |
| SN-36 | Filter Range Does Not Expand Upward | Create filter starting from A5, add data in A1:A4 | Filter range does not expand upward to include rows above |
| SN-37 | Apply Filter to Multi-Cell Selection | Select range A1:D20 (multiple rows and columns), create filter | Filter applies to entire selected area with headers in row 1 |
| SN-38 | Filter Range Shrinks on Column Delete | Create filter on A1:D10, delete column B | Filter range adjusts to A1:C10 |
| SN-39 | Filter Range Shrinks on Row Delete | Create filter on A1:C10, delete row 5 | Filter range adjusts to A1:C9 |
| SN-40 | Filter Range Expands on Column Insert | Create filter on A1:C10, insert column between A and B | Filter range expands to A1:D10 |
| SN-41 | Filter Range Expands on Row Insert | Create filter on A1:C10, insert row at position 5 | Filter range expands to A1:C11 |
| SN-42 | Extend Filter Range by Dragging | Create filter on A1:C10, drag blue corner handle to extend to A1:E15 | Filter range expands to new selection, new columns get filter headers |
| SN-43 | Auto Range Detection Stops at Empty Row | Click cell A1 with data in A1:C10, empty row 11, data in A12:C15, create filter | Filter auto-detects range A1:C10 only; rows 12-15 are not included in filter |

### 2.8 Filter and Hide/Unhide Interaction

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-44 | Hide Column With Active Filter | Apply filter on columns A-C, then manually hide column B | Column B is hidden, filter continues to work on visible columns A and C |
| SN-45 | Hide Row With Active Filter | Apply filter, then manually hide a visible row using right-click > Hide row | Row is hidden regardless of filter state |
| SN-46 | Unhide Row Not Matching Filter | Hide row 5 manually, apply filter that excludes row 5 value, then unhide row 5 | Row 5 remains hidden because it doesn't match active filter criteria |
| SN-47 | Unhide Column With Active Filter | Hide column B, apply filter, then unhide column B | Column B becomes visible with filter dropdown in header |

### 2.9 Filter by Color - Dynamic Updates

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-48 | Change Cell Color After Color Filter Applied | Apply filter by fill color (red), then change a visible red cell to blue | Row with changed color becomes hidden immediately |
| SN-49 | Add Matching Color After Filter | Apply filter by fill color (red), then change an unfiltered cell to red | Row becomes visible as it now matches filter |
| SN-50 | Remove Color After Color Filter | Apply filter by fill color (red), then remove fill color from a red cell | Row becomes hidden as it no longer matches filter |
| SN-51 | Color Filter With No Matching Colors | Apply filter by color, then change all cells to different color | No rows displayed, filter shows empty result |

### 2.10 Filter UI Counters

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| SN-52 | Select All Counter | Open filter with 10 unique values, check "Select all" label | Label shows correct count (e.g., "Select all (10)") |
| SN-53 | Displaying Counter After Search | Open filter, search for partial text matching 3 values | "Displaying X of Y" shows correct filtered count (e.g., "Displaying 3 of 10") |
| SN-54 | Counter Updates on Data Change | Add new unique value to filtered column, reopen filter | Counter reflects new total count |
| SN-55 | Counter With Blanks | Column has 5 values + 3 empty cells, open filter | Counter correctly includes "(Blanks)" in total count |

---

## 3. Regression Tests (Full Coverage)

Comprehensive scenarios including edge cases, combinations, and potential breaking points.

### 3.1 Edge Cases - Empty/No Data

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-01 | Filter Empty Column | Apply filter on column with no data | Filter dropdown shows "(Blanks)" only |
| RG-02 | No Matching Values | Apply filter that matches no rows | No rows displayed, filter icon indicates active filter |
| RG-03 | All Rows Match | Apply filter where all rows match | All rows remain displayed |
| RG-04 | Filter Column With Only Blanks | Column contains only empty cells | "(Blanks)" option available |

### 3.2 Edge Cases - Search Within Filter

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-05 | Search Non-Existent Value | Search for value that doesn't exist in filter | "No matches" message or empty list |
| RG-06 | Search With Special Characters | Search using special chars (@, #, $, etc.) | Correct filtering by special characters |
| RG-07 | Search Case Sensitivity | Search with different case | Case-insensitive matching |
| RG-08 | Search Partial Match | Search partial text | Values containing partial text shown |

### 3.3 Edge Cases - Long/Special Values

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-09 | Very Long Text Values | Filter column with very long text (1000+ chars) | Filter handles long text correctly |
| RG-10 | Special Characters in Data | Filter data with Unicode, emojis, symbols | Correct filtering and display |
| RG-11 | Numbers as Text | Filter column with numeric values stored as text | Text conditions work correctly |
| RG-12 | Mixed Data Types | Column with text, numbers, dates mixed | All values appear in filter dropdown |

### 3.4 Edge Cases - Numeric Boundaries

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-13 | Very Large Numbers | Filter with very large numbers (10^10+) | Correct numeric comparison |
| RG-14 | Negative Numbers | Filter with negative number conditions | Correct handling of negative values |
| RG-15 | Decimal Numbers | Filter with decimal precision | Correct decimal comparison |
| RG-16 | Zero Value | Filter with "Is equal to 0" | Correctly identifies zero values |

### 3.5 Edge Cases - Date Boundaries

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-17 | Date Today/Tomorrow/Yesterday | Use relative date conditions | Correct relative date filtering |
| RG-18 | Date Range Edge | Filter date at exact boundary of range | Boundary dates included/excluded correctly |
| RG-19 | Different Date Formats | Column with various date formats | Filter recognizes all date formats |

### 3.6 Filter State Management

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-20 | Remove Filter | Remove active filter | All rows displayed again |
| RG-21 | Change Filter Without Clearing | Apply new filter over existing filter | New filter replaces old one |
| RG-22 | Multiple Filters Then Remove One | Apply filters to columns A, B, C, then remove B | Filters A and C remain active |
| RG-23 | Filter Persistence After Page Refresh | Apply filter, refresh page | Filter state preserved |

### 3.7 Filter Views

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-24 | Create Filter View | Create named filter view | Filter view saved and accessible |
| RG-25 | Switch Between Filter Views | Create multiple views, switch between them | Correct view applied on selection |
| RG-26 | Delete Filter View | Delete existing filter view | View removed, data returns to default |

### 3.8 Data Manipulation With Active Filter

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-27 | Add Row With Filter Active | Add new row while filter is active | Row appears/hides based on filter criteria |
| RG-28 | Edit Cell With Filter Active | Edit cell value to match/not match filter | Row visibility updates accordingly |
| RG-29 | Delete Filtered Row | Delete row while filter is active | Row deleted, filter remains active |
| RG-30 | Sort With Filter Active | Apply sort while filter is active | Filtered data sorted correctly |

### 3.9 UI/UX Edge Cases

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-31 | Large Dataset Filter | Apply filter on 10,000+ rows | Filter works without timeout |
| RG-32 | Many Unique Values | Column with 500+ unique values | Filter dropdown handles list correctly |
| RG-33 | Filter Icon State | Apply/remove filters | Filter icon correctly indicates active state |
| RG-34 | Keyboard Navigation | Navigate filter dialog with keyboard | Accessible keyboard navigation |
| RG-35 | Hidden Row Numbers Not Displayed | Apply filter that hides rows 3, 5, 7 | Row numbers 3, 5, 7 are not visible in the row header; visible rows show gaps in numbering (1, 2, 4, 6, 8...) |
| RG-36 | All Values Restored After Filter Removal | Apply filter hiding some rows, then remove filter completely | All rows visible again, row numbers sequential without gaps |

### 3.10 Combination Tests

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-37 | Filter + Color + Condition | Apply filter by value, color, and condition on different columns | All filters work together |
| RG-38 | Custom Formula With References | Custom formula referencing other columns | Formula evaluates correctly |
| RG-39 | Conflicting Filters | Apply filters that should show 0 rows | Empty result set displayed correctly |

### 3.11 Custom Formula Edge Cases

| ID | Test Name | Description | Expected Result |
|----|-----------|-------------|-----------------|
| RG-40 | Invalid Custom Formula Syntax | Enter custom formula with syntax error (e.g., "=A1>>" or "=INVALID(") | Error message displayed, filter not applied |
| RG-41 | Custom Formula Non-Boolean Result | Enter formula that returns non-boolean value (e.g., "=A1+1") | Error or unexpected behavior handled gracefully |
| RG-42 | Custom Formula Empty | Submit custom formula condition with empty formula field | Validation error, filter not applied |