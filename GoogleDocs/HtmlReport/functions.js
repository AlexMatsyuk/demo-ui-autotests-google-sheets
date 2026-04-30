
var myColor = ["#c0eec0", "#fed9d9", "#FBE87E"]; //green,red,yellow
var myStrokeColor = ["#7CCD7C", "#d42945", "#ffcc00"];

function copyToClipboard(xpath) {
    var el = document.createElement('textarea');
    el.value = GetElementsByXpath(xpath)[0].innerHTML;
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
}

function SH(id1, id2, textOnHide, textOnShow, otherClass) {
    if (document.getElementById(id1).className.includes('v')) {
        document.getElementById(id2).innerHTML = textOnHide;
        document.getElementById(id1).className = 'h';
    } else {
        document.getElementById(id2).innerHTML = textOnShow;
        document.getElementById(id1).className = 'v' + ' ' + otherClass;
    }
}

function SHs(id1, id2, textOnHide, textOnShow, otherClass) {
    if (document.getElementById(id1).className.includes('v')) {
        document.getElementById(id2).innerHTML = textOnHide;
        GetElementsByXpath("//*[contains(@id,'" + id1 + "') and not(@id='" + id2 + "')]").forEach(function(entry) {
            entry.className = 'h';
        });
    } else {
        document.getElementById(id2).innerHTML = textOnShow;
        GetElementsByXpath("//*[starts-with(@id,'" + id1 + "')]").forEach(function(entry) {
            entry.className = 'v' + ' ' + otherClass;
        });
    }
}

function FilterRows() {
    var xpath = "//tr[contains(@id,'rT')]";
    for (var i = 0; i < arguments.length; i++) {
        var filterText = GetElementsByXpath("//input[@id='filter" + arguments[i] + "']")[0].value;
        if (filterText) {
            xpath = xpath + "[./td[@id='r" + arguments[i] + "' and .//*[contains(text(),'" + filterText + "')]]]";
        }
    }
    ReplaceClass(xpath, "h", "v");
}

function SortRows(column) {
    var rows, switching, i, x, y, shouldSwitch, dir, switchCount = 0;
    switching = true;
    dir = "asc";
    while (switching) {
        switching = false;
        rows = GetElementsByXpath("//table[@id='ReportsTable']//tr[contains(@id,'rT') and contains(@class,'v')]");
        for (i = 0; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].querySelector('[id="r' + column + '"]').textContent;
            y = rows[i + 1].querySelector('[id="r' + column + '"]').textContent;
            if (dir == "asc") {
                if (x.toLowerCase() > y.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.toLowerCase() < y.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchCount ++;
        } else {
            if (switchCount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }

    rows = GetElementsByXpath("//table[@id='ReportsTable']//tr[contains(@id,'rT') and contains(@class,'v')]");
    for (i = 0; i < (rows.length - 1); i++) {
        var testId = rows[i].id.replace("rT ", "");
        var detailsRow = GetElementsByXpath("//table[@id='ReportsTable']//tr[@id='" + testId + "TsC']")[0];
        detailsRow.parentNode.insertBefore(detailsRow, rows[i].nextSibling)
    }
}

function ReplaceClass(xpath, oldClassPart, newClassPart) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        var node = elements[i];
        node.className = node.className.replace(oldClassPart, newClassPart);
    }
}

function ReplaceText(xpath, oldTextPart, newTextPart) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        var node = elements[i];
        node.textContent = node.textContent.replace(oldTextPart, newTextPart);
    }
}

function SetText(xpath, newText) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        var node = elements[i];
        node.textContent = newText;
    }
}

function ClearValue(xpath) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        elements[i].value = "";
    }
}

function CU(checkboxXpath, xpath) {
    var selected = GetElementsByXpath(checkboxXpath)[0].checked;
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        var node = elements[i];
        if (selected && !node.className.includes(" selected")) {
            node.className = node.className + " selected";
        } else if (!selected && node.className.includes(" selected")) {
            node.className = node.className.replace(" selected", "");
        }
    }
}

function CUAll(checkboxAllXpath, testCheckboxesXpath) {
    var checked = GetElementsByXpath(checkboxAllXpath)[0].checked;
    var elements = GetElementsByXpath(testCheckboxesXpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        elements[i].checked = checked;
    }
}

function UncheckAll(xpath) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        elements[i].checked = false;
    }
}

function SetTfsTestFilterCriteria(testsXpath, resultFieldXpath) {
    var elements = GetElementsByXpath(testsXpath);
    var x = "";
    for (var i = 0, n = elements.length; i < n; ++i) {
        x = x + "Name=" + elements[i].textContent + "|";
    }
    x = x.substring(0, x.length - 1);
    GetElementsByXpath(resultFieldXpath)[0].textContent = x;
    copyToClipboard(resultFieldXpath);
}

function GetElementsByXpath(xpath) {
    var result = document.evaluate(xpath,
        document.documentElement,
        null,
        XPathResult.ORDERED_NODE_ITERATOR_TYPE,
        null);
    var elements = [];
    if (result) {
        while ((node = result.iterateNext())) {
            elements.push(node);
        }
    }
    return elements;
}

function AddEventListener() {
    var button = document.getElementById('btn-download');
    button.addEventListener('click',
        function() {
            button.href = canvas.toDataURL('image/png');
        });
}

function show(id) {
    document.getElementById(id).style.visibility = "visible";
    document.getElementById(id).style.display = "block";
}

function hide(id) {

    document.getElementById(id).style.visibility = "hidden";
    document.getElementById(id).style.display = "none";
}

function OpenInNewWindow() {
    var largeImage = document.getElementById('floatingImage');
    var url = largeImage.getAttribute('src');
    window.open(url, 'Image');
}

function updateFloatingImage(url) {
    document.getElementById('floatingImage').src = url;
}

function download(filename, text) {
    console.log("download method");
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

function downloadCurrentHtml() {
    var currentUrl = window.location.href;
    var currentName = currentUrl.substring(currentUrl.lastIndexOf("/") + 1);
    var updatedName = currentName.substring(0, currentName.lastIndexOf(".")) + ".with_comments.html";
    download(updatedName, document.documentElement.innerHTML);
}

function saveComment(xpath) {
    console.log("saveComment mehtod");
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        elements[i].setAttribute("value", elements[i].value);
    }
}

function extractComment(xpath) {
    var elements = GetElementsByXpath(xpath);
    for (var i = 0, n = elements.length; i < n; ++i) {
        //elements[i].value = localStorage.getItem(elements[i].id);
        elements[i].value = elements[i].getAttribute("value");
    }
}