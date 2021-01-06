
	// Browser Detection
	isMac = (navigator.appVersion.indexOf("Mac")!=-1) ? true : false;
	NS4 = (document.layers) ? true : false;
	IEmac = ((document.all)&&(isMac)) ? true : false;
	IE4plus = (document.all) ? true : false;
	IE4 = ((document.all)&&(navigator.appVersion.indexOf("MSIE 4.")!=-1)) ? true : false;
	IE5 = ((document.all)&&(navigator.appVersion.indexOf("MSIE 5.")!=-1)) ? true : false;
	ver4 = (NS4 || IE4plus) ? true : false;
	NS6 = (!document.layers) && (navigator.userAgent.indexOf('Netscape')!=-1)?true:false;

	// Body onload utility (supports multiple onload functions)
	var gSafeOnload = new Array();
	function SafeAddOnload(f)
	{
		if (IEmac && IE4)  // IE 4.5 blows out on testing window.onload
		{
			window.onload = SafeOnload;
			gSafeOnload[gSafeOnload.length] = f;
		}
		else if  (window.onload)
		{
			if (window.onload != SafeOnload)
			{
				gSafeOnload[0] = window.onload;
				window.onload = SafeOnload;
			}		
			gSafeOnload[gSafeOnload.length] = f;
		}
		else
			window.onload = f;
	}
	
	function SafeOnload()
	{
		for (var i=0;i<gSafeOnload.length;i++)
			gSafeOnload[i]();
	}
	
	function SetHighLight( n ) {
				ClearAllHighLight();
				
				switch( n ) {
					case 1:
						document.getElementById("lnkHOME").className = "Current";
						break;
					case 2:
						document.getElementById("lnkHR").className = "Current";
						break;
					case 3:
						document.getElementById("lnkPRODUCTS").className = "Current";
						break;
					case 4:
						document.getElementById("lnkSALES").className = "Current";
						break;
					case 5:
						document.getElementById("lnkREPORTS").className = "Current";
						break;
					case 6:
						document.getElementById("lnkCORPORATE").className = "Current";
						break;
					case 7:
						document.getElementById("lnkFINANCE").className = "Current";
						break;
					case 8:
						document.getElementById("lnkMIS").className = "Current";
						break;
					case 9:
						document.getElementById("lnkADMIN").className = "Current";
						break;
					case 10:
						document.getElementById("lnkORGANIZATION").className = "Current";
						break;
					case 11:
					    document.getElementById("lnkSuppliers").className = "Current";
					    break;
					case 12:
					    document.getElementById("lnkDebtors").className = "Current";
					    break;
					case 13:
					    document.getElementById("lnkCommunications").className = "Current";
					    break;
				}							
			}
			
    function ClearAllHighLight() {
	    var x = document.getElementsByTagName('a');			
	    for (var i=0;i<x.length;i++) {					
		    if (x[i].className == 'Current') {					
				    x[i].className = '';
		    }			
	    }
    }
