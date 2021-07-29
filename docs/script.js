var releasesUrl="https://api.github.com/repos/hypergamesdev/glitchout/releases";
var releasesData;
var releasesDataJSON;
var stableUrl="https://api.github.com/repos/hypergamesdev/glitchout/releases/latest";
var stableUrlGoto="https://github.com/hypergamesdev/glitchout/releases/latest";
var stableData;
var stableTag;
var stableName;
var latestUrlGoto="https://github.com/HyperGamesDev/glitchout/releases/tag/";
var latestData;
var latestTag;
var latestName;
var launcherUrl="https://github.com/HyperGamesDev/glitchout/releases/download/v0.2/Glitchout-BETALauncher-0.2.zip";
async function getapiReleases(){
	const responseReleases=await fetch(releasesUrl);
	releasesData=await responseReleases.json();
	
	const responseStable=await fetch(stableUrl);
	stableData=await responseStable.json();
	
	console.log("Releases Data");
	console.log(releasesData);
	latestData=releasesData[0];
	console.log("Latest Data");
	console.log(latestData);
	latestTag=latestData.tag_name;
	latestName=(latestData.name).replace("glitchout","");
	latestName=latestName.replace("(","<br>(");
	
	console.log("Stable Data");
	console.log(stableData);
	stableName=(stableData.name).replace("glitchout","");
	
	//sleep(100);
	setReleasesText();
	setReleasesHref();
	setAudio();
}
getapiReleases();


function setReleasesText(){
	//document.getElementById("stableText").innerHTML=stableName;
	//document.getElementById("latestText").innerHTML=latestName;
}
function setReleasesHref(){
	document.getElementById("launcher").href=launcherUrl;
	//document.getElementById("stable").href=stableUrlGoto;
	//latestUrlGoto=latestUrlGoto.concat(latestTag);
	//document.getElementById("latest").href=latestUrlGoto;
}

function downloadLauncher(){
	location.href=launcherUrl;
}
function goToStableRelease(){
	location.href=stableUrlGoto;
}
function goToLatestRelease(){
	latestUrlGoto=latestUrlGoto.concat(latestTag);
	location.href=latestUrlGoto;
}

function setAudio(){
	var a=document.getElementsByTagName("a");
	a[0].onmouseover="playHover();";a[0].onclick="playClick();";a[0].allow="autoplay";
	//for(var i=0;i<a.length;i++){a[i].onmouseover="playHover();";a[i].onclick="playClick();";a[i].allow="autoplay";}
}
function playHover(){
	var audio = new Audio('audio/ButtonHover.wav');
	audio.currentTime=0;
	audio.play();
}function playClick(){
	var audio = new Audio('audio/ButtonClick.wav');
	audio.play();
}


function sleep(milliseconds) {
  const date = Date.now();
  let currentDate = null;
  do {
    currentDate = Date.now();
  } while (currentDate - date < milliseconds);
}