function SetBannerHighLight( n ) {
	try{
		top.header.SetHighLight( n );
	}catch(e)
	{}	
}


//Center window	'haveScroll = yes | no
function jsWinOpen(x,w,h, haveScroll){
	var winLeft = (screen.width - w) / 2;
	var winUp = (screen.height - h) / 2;
	if (! window.focus)
		return true;
		
	haveScroll = 'yes';
	if( haveScroll == "" )
		haveScroll = "no";
		
	window.open(x,"","width=" + w + ",height=" + h + ",top="+ winUp+",left="+ winLeft +",resizable=yes,status=yes,menubar=no,scrollbars=" + haveScroll);	
}

function jsWinOpen2(x,w,h, haveScroll){
	var winLeft = (screen.width - w) / 2;
	var winUp = (screen.height - h) / 2;
	if (! window.focus)
		return true;
		
	haveScroll = 'yes';
		
	window.open(x,"","width=" + w + ",height=" + h + ",top="+ winUp+",left="+ winLeft +",resizable=yes,status=yes,menubar=no,scrollbars=" + haveScroll);
}

function jsOpenOperationalReport( url )
{
	jsWinOpen2( sDOMAIN+ "/" + url, 795, 580, 'yes');
}

function jsOpenReport( url )
{
	jsWinOpen( sDOMAIN+ "/" + url, 795, 580, 'no');
}

