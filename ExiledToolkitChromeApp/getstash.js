document.addEventListener('DOMContentLoaded', function(){
    document.getElementById('hardcorebutton').onclick=function(){insertStashJson('hardcore', 0);};
});

var arr = [];
var arrinserts = 0;
var tabcount = 4;
var cont = true;
var firstrun = true;
var leagueid = '';

function handleStateChange()
{
    if (this.readyState == 4) {
        var thing = JSON.parse(this.responseText);
        var jsonstring = '';
        if (thing.error == null)
        {
            arr[arrinserts] = {type: 'stash', items: thing.items};
            arrinserts = arrinserts + 1;
            jsonstring = JSON.stringify(arr);
            tabcount = thing.numTabs;
        }
        else
        {
            jsonstring = "Error " + tabcount + " " + this.responseText;
        }
        if (firstrun)
        {
            firstrun = false;
            insertRemainingStashJson();
        }
        $("#debug").append(arrinserts + "," + tabcount)
        if (arrinserts >= tabcount)
        {
            $('#jsonframe').contents().find('html').html(jsonstring);
            $('#jsonframe').toggle();
        }
    }
}

function insertStashJson(inleagueid){
    leagueid = inleagueid;
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = handleStateChange;
    xhr.open("GET", 'http://www.pathofexile.com/character-window/get-stash-items?league=' + leagueid)
    xhr.send();
}

function insertRemainingStashJson() {
    var ms = 2500;
    for (var i=1;i<tabcount;i++)
    {
        var x = 1;
        setTimeout(function() {
            var reqString = 'http://www.pathofexile.com/character-window/get-stash-items?league=' + leagueid + '&tabIndex=' + x;
            x = x + 1;
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = handleStateChange;
            xhr.open("GET", reqString)
            xhr.send();
        }, ms)
        ms = ms + 2500
    }
}