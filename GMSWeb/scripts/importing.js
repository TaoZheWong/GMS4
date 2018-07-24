var progressEnd = 9; // set to number of progress <span>'s.
var progressColor = 'orange'; // set to progress bar color
var progressInterval = 100; // set to time between updates (milli-seconds)

var progressAt = progressEnd;
var progressTimer;
function progress_clear() {
for (var i = 1; i <= progressEnd; i++) document.getElementById('progress'+i).style.backgroundColor = 'transparent';
progressAt = 0;
}
function progress_update() {



document.getElementById('showbar').style.visibility = 'visible';
document.getElementById('importing').style.visibility = 'visible';
progressAt++;
if (progressAt > progressEnd) progress_clear();
else document.getElementById('progress'+progressAt).style.backgroundColor = progressColor;
progressTimer = setTimeout('progress_update()',progressInterval);
}
function progress_stop() {
clearTimeout(progressTimer);
progress_clear();

document.getElementById('showbar').style.visibility = 'hidden';
document.getElementById('importing').style.visibility = 'hidden';

}

