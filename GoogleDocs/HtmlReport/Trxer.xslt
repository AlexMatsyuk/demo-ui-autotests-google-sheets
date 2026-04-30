<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:t="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" 
                version="2.0">
    
    <xsl:output method="html" indent="yes"/>
    <xsl:key name="TestMethods" match="t:TestMethod" use="@className"/>
    
    <xsl:template match="/" >
        <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
        <html>
            <head>
                <meta charset="utf-8"/>
                <link rel="stylesheet" type="text/css" href="Trxer.css"/>
                <link rel="stylesheet" type="text/css" href="TrxerTable.css"/>
                <script language="javascript" type="text/javascript" src="functions.js"></script>
                <title>
                    <xsl:value-of select="/t:TestRun/@name"/>
                </title>
            </head>
            <body>
                <div id="divToRefresh" class="wrapOverall">
                    <div id="floatingImageBackground" class="floatingImageBackground" style="visibility: hidden;">
                        <strong style="font-size: 20px;">Click image to view in new browser tab</strong>
                        <div class="floatingImageCloseButton" onclick="hide('floatingImageBackground');"></div>
                        <img src="" id="floatingImage" onclick="OpenInNewWindow();"/>
                    </div>
                    <xsl:variable name="testRunOutcome" select="t:TestRun/t:ResultSummary/@outcome"/>
                    
                    <div class="SummaryDiv">
                        <table class="DetailsTable StatusesTable">
                            <caption style="min-width: 150px;">Tests Statuses</caption>
                            <tbody>
                                <tr class="odd">
                                    <th class="c statusCount">Total</th>
                                    <td class="statusCount">
                                        <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@total" />
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row" class="c statusCount">Passed</th>
                                    <td class="statusCount">
                                        <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@passed" />
                                    </td>
                                </tr>
                                <tr>
                                    <th scope="row" class="c statusCount">Failed</th>
                                    <td class="statusCount">
                                        <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@failed" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table class="SummaryTable">
                            <caption>Run Time Summary</caption>
                            <tbody>
                                <tr class="odd">
                                    <th class="c">Start Time</th>
                                    <td>
                                        <xsl:value-of select="/t:TestRun/t:Times/@start"/>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="c">End Time</th>
                                    <td>
                                        <xsl:value-of select="/t:TestRun/t:Times/@finish"/>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="c">Duration</th>
                                    <td>
                                        <xsl:value-of select="/t:TestRun/t:Times/@queuing"/>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <xsl:if test="/t:TestRun/t:TestSettings/t:AdditionalData/t:key">
                            <table class="DetailsTable">
                                <caption>Tests Details</caption>
                                <tbody>
                                    <xsl:for-each select="/t:TestRun/t:TestSettings/t:AdditionalData/t:key">
                                        <xsl:variable name="dataValue">
                                            <xsl:value-of select="text()" disable-output-escaping="yes"/>
                                        </xsl:variable>
                                        <xsl:if test="$dataValue !=''">
                                            <tr>
                                                <th scope="row" class="c" style="width:15%"><xsl:value-of select="@name"/></th>
                                                <td>
                                                    <xsl:choose>
                                                        <xsl:when test="contains($dataValue, 'http') and (string-length($dataValue) > 50)">
                                                            <a target="_blank" href='{$dataValue}'>click to open</a>
                                                        </xsl:when>
                                                        <xsl:when test="contains($dataValue, 'http') and (not(string-length($dataValue) > 50))">
                                                            <a target="_blank" href='{$dataValue}'><xsl:value-of select="$dataValue"/></a>
                                                        </xsl:when>
                                                        <xsl:otherwise>
                                                            <xsl:value-of select="$dataValue"/>
                                                        </xsl:otherwise>
                                                    </xsl:choose>
                                                </td>
                                            </tr>
                                        </xsl:if>
                                    </xsl:for-each>
                                </tbody>
                            </table>
                        </xsl:if>
                    </div>
                    <xsl:variable name="testsSet" select="//t:TestRun/t:Results/t:UnitTestResult" />
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    <a id="saveAll" class="FiterResults" style="background-color:#04671d;" onclick='saveComment("//input[@class=\"ci\"]");
                                                      saveComment("//textarea[@class=\"ci\"]");
                                                      downloadCurrentHtml();' type="button">
                                        <div class="m">Save changes and comments</div>
                                    </a>
                                    <a class="FiterResults" style="background-color:#04671d;" onclick='extractComment("//input[@class=\"ci\"]");
                                                      extractComment("//textarea[@class=\"ci\"]")' type="button">
                                        <div class="m">Show comments</div>
                                    </a>
                                </td>
                                <td style="width: 100%;">
                                    <textarea name="server" id="mainComment" class="ci" type="text" style="width: 100%; height: 90px; color: #e60909;"
                                              placeholder="Type comments here or in the row for particular test. Click 'Save changes and comments' button, new .html report with changes will be downloaded. Now comments are saved and you can share newly generated html report with other people, comments will appear by clicking 'Show comments' button."/>
                                </td>
                            </tr>
                            <tr>
                                <td style="min-width: 250px;">
                                    <a class="FiterResults" style="background-color:#072563ed;" 
                                       onclick='SetTfsTestFilterCriteria("//tr[contains(@id,\"rT\") and contains(@class,\"selected\")]//div[contains(@class,\"m \")]", "//span[@id=\"tfsTestFilter\"]")'>
                                        <div class="m">Generate Tests filter <br/> and copy to clipboard</div>
                                    </a>
                                </td>
                                <td style="width:100%">
                                    <span id="tfsTestFilter" class="TestsFilterCriteria">Check test cases and click button "Generate Tests filter".
                                        See filter rules
                                        <a target="_blank" href="https://github.com/Microsoft/vstest-docs/blob/master/docs/filter.md">here</a>
                                        and 
                                        <a target="_blank" href="https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=netcore21#filter-option-details">here</a>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <a class="FiterResults" onclick='ReplaceClass("//tr[contains(@id,\"rT\")]", "h", "v");
                                                   ClearValue("//input[contains(@id,\"filter\")]");'>
                                        <div class="m">Show All Tests</div>
                                    </a>
                                </td>
                                <td>
                                    <a class="FiterResults" onclick='ReplaceClass("//tr[contains(@id,\"rT\")][contains(@class,\"testPassed\")]", "v", "h");
                                                   ReplaceClass("//tr[contains(@id,\"TsC\")][./preceding-sibling::tr[1][contains(@id,\"rT\") and contains(@class,\"testPassed\")]]", "v", "h");
                                                   ClearValue("//input[contains(@id,\"filter\")]");'>
                                        <div class="m">Hide Passed Tests</div>
                                    </a>
                                    <a class="FiterResults" onclick='UncheckAll("//tr[contains(@id,\"rT\") and contains(@class,\"selected\") and contains(@class,\"v\")]//input[@type=\"checkbox\"]");
                                                   ReplaceClass("//tr[contains(@id,\"rT\") and contains(@class,\"selected\")]", "v", "h");
                                                   ReplaceClass("//tr[contains(@id,\"TsC\")][./preceding-sibling::tr[1][contains(@id,\"rT\") and contains(@class,\"selected\")]]", "v", "h");
                                                   ClearValue("//input[contains(@id,\"filter\")]");
                                                   ReplaceClass("//tr[contains(@id,\"rT\") and contains(@class,\"selected\")]", " selected", "");
                                                   '>
                                        <div class="m">Hide and Uncheck Selected Tests</div>
                                    </a>
                                    <a class="FiterResults" onclick='ReplaceClass("//tr[contains(@id,\"TsC\")]", "v", "h");
                                                   ReplaceText("//div[@class=\"om\"]/div[contains(@class,\"m\")]", "Hide ", "");'>
                                        <div class="m">Collapse All</div>
                                    </a>
                                    <a class="ShowRules" onclick='ReplaceClass("//tr[contains(@id,\"infoRow\")]", "h", "v");cursor:pointer;'>click here for help</a>
                                </td>
                            </tr>
                            <tr id="infoRow" class="h">
                                <td/>
                                <td>
                                    <p style="text-align: left; padding: 10px 10px 10px 10px;">
                                        1. Don't use IE. Reports work in Chrome and Firefox browser only. Chrome is preferrable.
                                        <br/>
                                        2. You can click TestMethod name to view logs.
                                        <br/>
                                        3. When logs are expanded, additionally you can click image icon to view screenshot, or "Show Stacktrace link" to view stacktrace, it they are present (usually stacktrace and screenshot are displayed in the failed tests only).
                                        <br/>
                                        4. Button "Hide Passed Tests" does not uncheck passed rows, it just hides them.
                                        <br/>
                                        5. You can click Column titles marked by "↕" sign to sort. Sorting can be applied only by 1 column. Sorting is applied for visible rows only. For ~120 rows sorting may take about 5 seconds, during which page is unresponsive.
                                        <br/>
                                        6. You can type search keyword in multiple search-boxes and the click "filter" button. Results, that correspond to all the specified search keywords, will be displayed only.
                                        <br/>
                                        7. When you click "Generate TestCaseFilter" or "Generate Tests filter", all the tests with checked checkboxes will be included, even invisible rows.
                                        <br/>
                                        8. Checkbox Check / Uncheck All will check and uncheck only visible rows. If you wish to check/uncheck invisible rows, click "Show All Tests" before.
                                        <br/>
                                        9. You can type notes and comments in the textarea to say something important about whole test run, or in the row for particular test. After "Save changes and comments" button is clicked, comments are saved in the new html report, report is downloaded automatically. Now you can share this new report with other people - they will be able to view your comments by clicking "Show comments" button.
                                    </p>
                                    <a class="ShowRules" onclick='ReplaceClass("//tr[contains(@id,\"infoRow\")]", "v", "h");cursor:pointer;'>hide</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table id="ReportsTable">
                        <thead>
                            <tr class="odd">
                                <th scope="col" abbr="Test">
                                    <input type="checkbox" class="checkboxAll" onclick='CUAll("//input[@class=\"checkboxAll\"]","//tr[contains(@id,\"rT\")][contains(@class,\"v\")]//input[@class=\"testCheckbox\"]");
                  CU("//input[@class=\"checkboxAll\"]","//tr[contains(@id,\"rT\")][contains(@class,\"v\")]")' />
                                </th>
                                <th id="columnTestMethod" scope="col" abbr="Test">
                                    <div onclick="SortRows('TM')" style="cursor: pointer;">Test Method↕</div>
                                </th>
                                <th id="columnTestClass" scope="col">
                                    <div onclick="SortRows('TC')" style="cursor: pointer;">Test Class↕</div>
                                </th>
                                <th id="columnTestCase" scope="col">
                                    <div onclick="SortRows('TstCs')" style="cursor: pointer;">Test Case↕</div>
                                </th>
                                <th id="columnBug" scope="col">
                                    <div onclick="SortRows('Bg')" style="cursor: pointer;">Bug↕</div>
                                </th>
                                <th id="columnCategories" scope="col">
                                    <div onclick="SortRows('TstCatgrs')" style="cursor: pointer;">Categories↕</div>
                                </th>
                                <th id="columne" scope="col">
                                    <div onclick="SortRows('E')" style="cursor: pointer;">Error Message and notes↕</div>
                                </th>
                                <th id="columnStartTime" scope="col">
                                    <div onclick="SortRows('ST')" style="cursor: pointer;">Start Time↕</div>
                                </th>
                                <th id="columnDuration" scope="col">
                                    <div onclick="SortRows('D')" style="cursor: pointer;">Duration↕</div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td style="padding:3px;">
                                    <button onclick='ReplaceClass("//tr[contains(@id,\"rT\")]", "v", "h");
                                    ReplaceClass("//tr[contains(@id,\"TsC\")]", "v", "h");
                                    ReplaceClass("//tr[contains(@id,\"Stacktrace\")]", "v", "h");
                                    FilterRows("TM", "TC", "TstCs", "Bg", "TstCatgrs", "E", "ST", "D");'
                                            style="font-size: 17px;float: right;cursor: pointer;width: 100%;padding: 0px 0px 0px 0px;">filter</button>
                                </td>
                                <td>
                                    <input id="filterTM" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterTC" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterTstCs" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterBg" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterTstCatgrs" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterE" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterST" style="width: 100%;"/>
                                </td>
                                <td>
                                    <input id="filterD" style="width: 100%;"/>
                                </td>
                            </tr>


                            <xsl:for-each select="$testsSet">
                                <xsl:variable name="testId" select="@testId" />

                                <xsl:variable name="rowVisibility">
                                    <xsl:choose>
                                        <xsl:when test="@outcome = 'Failed'">v</xsl:when>
                                        <xsl:otherwise>h</xsl:otherwise>
                                    </xsl:choose>
                                </xsl:variable>

                                <xsl:variable name="rowId">rT <xsl:value-of select="generate-id(@testId)"/></xsl:variable>
                                <tr id="{$rowId}" class="{$rowVisibility} test{@outcome}">
                                    <xsl:variable name="outputLog" select="t:Output/t:StdOut"/>
                                    <th class="c{@outcome}">
                                        <input type="checkbox" class="testCheckbox" onclick='CU("//tr[@id=\"{$rowId}\"]//input","//tr[@id=\"{$rowId}\"]")' />
                                    </th>
                                    <td id="rTM" scope="row" class="c{@outcome}">
                                        <div class="om" onclick="SH('{generate-id(@testId)}TsC','{generate-id(@testId)}Button','{@testName}','{@testName}');">
                                            <xsl:choose>
                                                <xsl:when test="@outcome = 'Passed'">
                                                    <div class="m tp" id="{generate-id(@testId)}Button"><xsl:value-of select="@testName" /></div>
                                                </xsl:when>
                                                <xsl:otherwise>
                                                    <div class="m testNameFailed" id="{generate-id(@testId)}Button"><xsl:value-of select="@testName" /></div>
                                                </xsl:otherwise>
                                            </xsl:choose>
                                        </div>
                                    </td>
                                    <td id="rTC" class="lt">
                                        <span>
                                            <xsl:value-of select="//t:TestRun/t:TestDefinitions/t:UnitTest[@id=$testId]/t:TestMethod/@className" />
                                        </span>
                                    </td>
                                    <td id="rTstCs">
                                        <span>
                                            <xsl:variable name="testCaseUrl" select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:StdOut/t:testCase/@href" />
                                            <a target="_blank" href="{$testCaseUrl}">
                                                <xsl:value-of select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:StdOut/t:testCase/@id" />
                                            </a>
                                        </span>
                                    </td>
                                    <td id="rBg">
                                        <xsl:variable name="bugsLinks" select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:StdOut/t:bug" />
                                        <xsl:for-each select="$bugsLinks">
                                            <xsl:variable name="bugId" select="@id" />
                                            <xsl:variable name="bugHref" select="@href" />
                                            <span>
                                                <a target="_blank" href="{$bugHref}">
                                                    <xsl:value-of select="$bugId" />
                                                </a>. 
                                            </span>
                                        </xsl:for-each>
                                    </td>
                                    <td id="rTstCatgrs">
                                        <span>
                                            <xsl:value-of select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:StdOut/t:categories/@value"/>
                                        </span>
                                    </td>
                                    <td id="rE" class="lt">
                                        <xsl:choose>
                                            <xsl:when test="string-length(t:Output/t:ErrorInfo/t:Message/text()) > 400">
                                                <span size="2" class="v l">
                                                    <xsl:value-of select="substring(t:Output/t:ErrorInfo/t:Message/text(), 1, 400)" /> ...
                                                    <strong style="color: red;">(expand test to see full error message)</strong>
                                                    <input id="{$rowId} comment" class="ci" type="text" style="color: #e60909;"/>
                                                </span>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <span size="2" class="v l">
                                                    <xsl:value-of select="t:Output/t:ErrorInfo/t:Message/text()" />
                                                    <input id="{$rowId} comment" class="ci" type="text" style="color: #e60909;"/>
                                                </span>
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </td>
                                    <td id="rST" class="lt">
                                        <span>
                                            <xsl:value-of select="substring(@startTime, 12, 8)"/>
                                        </span>
                                    </td>
                                    <td id="rD" class="lt">
                                        <span>
                                            <xsl:value-of select="substring(@duration, 4, 9)" />
                                        </span>
                                    </td>
                                </tr>
                                <tr id="{generate-id(@testId)}TsC" class="h">
                                    <td>
                                        <xsl:call-template name="imageExtractor">
                                            <xsl:with-param name="testId" select="$testId" />
                                        </xsl:call-template>
                                    </td>
                                    <td>
                                        <xsl:value-of select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/@computerName"/>
                                    </td>
                                    <td colspan="7">
                                        <xsl:for-each select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
                                            <xsl:call-template name="tDetails">
                                                <xsl:with-param name="testId" select="@testId" />
                                                <xsl:with-param name="description">All</xsl:with-param>
                                            </xsl:call-template>
                                        </xsl:for-each>
                                    </td>
                                </tr>

                                <xsl:choose>
                                    <xsl:when test="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:ErrorInfo/t:StackTrace">
                                        <xsl:for-each select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
                                            <xsl:call-template name="tStackTrace">
                                                <xsl:with-param name="testId" select="@testId" />
                                                <xsl:with-param name="description">All</xsl:with-param>
                                            </xsl:call-template>
                                        </xsl:for-each>
                                    </xsl:when>
                                </xsl:choose>
                                
                            </xsl:for-each>
                        </tbody>
                    </table>
                </div>
            </body>
        </html>
    </xsl:template>


    <xsl:template name="tStatus">
        <xsl:param name="testId" />
        <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
            <xsl:choose>
                <xsl:when test="@outcome='Passed'">
                    <td class="passed">PASSED</td>
                </xsl:when>
                <xsl:when test="@outcome='Failed'">
                    <td class="failed">FAILED</td>
                </xsl:when>
                <xsl:when test="@outcome='Inconclusive'">
                    <td class="warn">Inconclusive</td>
                </xsl:when>
                <xsl:when test="@outcome='Timeout'">
                    <td class="failed">Timeout</td>
                </xsl:when>
                <xsl:when test="@outcome='Error'">
                    <td class="failed">Error</td>
                </xsl:when>
                <xsl:when test="@outcome='Warn'">
                    <td class="warn">Warn</td>
                </xsl:when>
                <xsl:otherwise>
                    <td class="info">OTHER</td>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
    </xsl:template>


    <xsl:template name="tDetails">
        <xsl:param name="testId" />
        <xsl:param name="description" />
        <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
            <xsl:call-template name="debugInfo">
                <xsl:with-param name="testId" select="$testId" />
                <xsl:with-param name="description" select="$description" />
            </xsl:call-template>
        </xsl:for-each>
    </xsl:template>

    <xsl:template name="imageExtractor">
        <xsl:param name="testId" />
        <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output">

            <xsl:variable name="screenshotLink" select="t:StdOut/t:test//t:screenshot"/>
                <xsl:choose>
                    <xsl:when test="$screenshotLink">
                        <div class="atachmentImage" onclick="show('floatingImageBackground');updateFloatingImage('{$screenshotLink/@href}');"></div>
                    </xsl:when>
                 </xsl:choose>

        </xsl:for-each>
    </xsl:template>

    <xsl:template name="tStackTrace">
        <xsl:param name="testId" />
        <xsl:param name="description" />
        <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
            <xsl:choose>
                <xsl:when test="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:ErrorInfo/t:StackTrace">
                    <tr id="{generate-id($testId)}{$description}Stacktrace" class="h">
                        <td class="ex" colspan="9">
                            <pre>
                                <xsl:value-of select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:ErrorInfo/t:StackTrace" />
                            </pre>
                        </td>
                    </tr>
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
    </xsl:template>






    <xsl:template name="debugInfo">
        <xsl:param name="testId" />
        <xsl:param name="description" />
        <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output">

            <xsl:variable name="MessageErrorStacktrace" select="t:ErrorInfo/t:StackTrace"/>
            <xsl:variable name="StdOut" select="t:StdOut"/>

            <xsl:if test="$StdOut or $MessageErrorStacktrace">
                <xsl:for-each select="$StdOut/t:test/t:step">
                    <xsl:variable name="testStepId" select="@id"/>
                    <xsl:variable name="testStepName" select="$StdOut/t:test/t:step[@id=$testStepId]/@name" />
                    
                    <xsl:choose>
                        <xsl:when test="$StdOut/t:test/t:step[@id=$testStepId]/t:stepData">
                            <table>
                                <xsl:for-each select="$StdOut/t:test/t:step[@id=$testStepId]/t:stepData">
                                    <tr>
                                        <td class="sdk"><xsl:value-of select="@id"/></td>
                                        <td class="sdv"><xsl:value-of select="@name"/><br/></td>
                                    </tr>
                                </xsl:for-each>
                            </table>
                        </xsl:when>
                    </xsl:choose>

                    <xsl:choose>
                        <xsl:when test="$StdOut/t:test/t:step[@id=$testStepId]/t:error">
                            <div class="OpenFailedStepButton"
                                 onclick="SHs('{generate-id(@id)}','Button{generate-id(@id)}','▲ Step {@id} :: {@name}','▼ Step {@id} :: {@name}', 'l');">
                                <div class="m testNameFailed" id="Button{generate-id(@id)}">▲ Step
                                    <xsl:value-of select="$testStepId"/> ::
                                    <xsl:value-of select="$testStepName"/>
                                </div>
                            </div>
                        </xsl:when>
                        <xsl:otherwise>
                            <div class="os"
                                 onclick="SHs('{generate-id(@id)}','Button{generate-id(@id)}','▲ Step {@id} :: {@name}','▼ Step {@id} :: {@name}', 'l');"
                                 id="Button{generate-id(@id)}">▲ Step
                                <xsl:value-of select="$testStepId"/> ::
                                <xsl:value-of select="$testStepName"/>
                            </div>
                        </xsl:otherwise>
                    </xsl:choose>

                    <xsl:for-each select="$StdOut/t:test/t:step[@id=$testStepId]/t:span">
                        <xsl:if test="@jsonData">
                            <xsl:variable name="jsonButtonId" select="generate-id()"/>
                            <div id='{generate-id($testStepId)}-{$jsonButtonId}-JL' class="h"
                                 onclick="SH('{$jsonButtonId}-{generate-id($testStepId)}-JD','{generate-id($testStepId)}-{$jsonButtonId}-JL','+ json','- json', 'l');"
                                 style="color:#00f;font-weight: bold;cursor: pointer;">+ json
                            </div>
                            <a id='{generate-id($testStepId)}-{$jsonButtonId}-JDC' class="h" href="javascript:;" 
                               onclick='copyToClipboard("//span[@id=\"{$jsonButtonId}-{generate-id($testStepId)}-JD\"]")' 
                               style="font-weight: bold; font-size: 13px;">Copy Json
                            </a>
                            <span id='{$jsonButtonId}-{generate-id($testStepId)}-JD' class="h" 
                                  onclick="Stringify('{$jsonButtonId}-{generate-id($testStepId)}-JD')" 
                                  style="white-space: pre; word-spacing: 7px; font-size: small; margin: 1px; line-height: 0.2; color: #339833;">
                                <xsl:value-of select="text()"/>
                            </span>
                        </xsl:if>
                        <xsl:if test="not(@jsonData)">
                            <span id='{generate-id($testStepId)}' class="h">
                                <xsl:value-of select="text()"/>
                            </span>
                        </xsl:if>
                    </xsl:for-each>
                                        
                </xsl:for-each>
                <xsl:if test="not(/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:StdOut/t:test/t:step)">
                    <span class="l" >
                        <xsl:value-of select="$StdOut/t:test"/>
                    </span>
                </xsl:if>
                <xsl:if test="$StdOut">
                    <br/>
                </xsl:if>
            </xsl:if>
            <xsl:value-of select="t:StdErr" />
            <xsl:variable name="StdErr" select="t:StdErr"/>
            <xsl:if test="$StdErr">
                <xsl:value-of select="$StdErr"/>
                <br/>
            </xsl:if>
            <xsl:variable name="MessageErrorInfo" select="t:ErrorInfo/t:Message"/>
            <xsl:if test="$MessageErrorInfo">
                <font class="e" style='color: #880B0B; background-color: #F3E0E0;' size="2"><b class="v l"><xsl:value-of select="$MessageErrorInfo"/></b></font>
            </xsl:if>
            <xsl:if test="$MessageErrorStacktrace">
                <a class="stackTrace" id="{generate-id($testId)}{$description}StacktraceToggle" href="javascript:SH('{generate-id($testId)}{$description}Stacktrace','{generate-id($testId)}{$description}StacktraceToggle','Show Stacktrace','Hide Stacktrace', 'stackTraceLink');">Show Stacktrace</a>
                <a class="stackTrace" href="javascript:;" onclick='copyToClipboard("//tr[@id=\"{generate-id($testId)}AllStacktrace\"]//pre")'>Copy Stacktrace</a>
            </xsl:if>

        </xsl:for-each>
    </xsl:template>
    
</xsl:stylesheet>