var ensureOpenerIsLoggedOut=function(){try{if(window.opener!=null&&window!=window.opener&&window.opener.document.habboLoggedIn!=null){if(window.opener.document.habboLoggedIn==true){window.opener.location.replace(window.opener.location.href)}}}catch(a){}};$(document).ready(function(){if(logoutVariables.popup){ensureOpenerIsLoggedOut()
}if(logoutVariables.popup||logoutVariables.embedded){var i=$("#header-content a");if(i&&i[0]){$(i[0]).prop("target","_blank")}}$("#logout-ok").click(function(){c(this);if(logoutVariables.popup){window.close()}else{if(logoutVariables.embedded){document.location.href=logoutVariables.serverRoot+"/embed"
}else{document.location.href=logoutVariables.serverRoot}}return false});$("#facebook-wall-logout-ok").click(function(n){c(this);top.location.href=logoutVariables.fbAppWallURL;return false});if(logoutVariables.rpxLogout){$("#rpx-logout-ok").click(function(){de=document.documentElement;w=window.innerWidth||self.innerWidth||(de&&de.clientWidth)||document.body.clientWidth;
h=window.innerHeight||self.innerHeight||(de&&de.clientHeight)||document.body.clientHeight;var n="toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width="+w+",height="+h;var o=window.open(logoutVariables.providerURL,null,n);o.focus();return false})}Cookie.erase("habboclient");
Cookie.erase("friendlist");var c=function(n){if(!l(n)||b){return false}b=true;$.ajax({type:"POST",url:logoutVariables.serverRoot+"/exit_poll",data:({userId:parseInt(logoutVariables.userId),hash:logoutVariables.hash,pollId:e,answers:JSON.stringify(j)}),success:m,error:k});return true};var d=function(){if(logoutVariables.rpxLogout){de=document.documentElement;
w=window.innerWidth||self.innerWidth||(de&&de.clientWidth)||document.body.clientWidth;h=window.innerHeight||self.innerHeight||(de&&de.clientHeight)||document.body.clientHeight;var n="toolbar=no,location=yes,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width="+w+",height="+h;var o=window.open(logoutVariables.providerURL,null,n);
o.focus();return false}else{if(logoutVariables.faceBookLogin){top.location.href=logoutVariables.fbAppWallURL;return false}else{if(logoutVariables.popup){window.close()}else{if(logoutVariables.embedded){document.location.href=logoutVariables.serverRoot+"/embed"}else{document.location.href=logoutVariables.serverRoot
}}return false}}};var b=false;var m=function(p){var n=false;var o=$("#poll-"+e);o.children(":not(img)").fadeOut("slow",function(){if(!n){o.css("border-radius","12px 12px 12px 0px");o.children("img").css("margin-top","35px");o.append("<span>"+p+"</span>")}n=true});b=false;d()};var k=function(){d();b=false
};var j=[];var e=null;$(".poll-radio").click(function(){e=parseInt($(this).closest(".poll").attr("id").replace("poll-",""));var n=parseInt($(this).parent().attr("id").replace("question-",""));var o=parseInt($(this).text());$(this).attr("check",true);f({questionId:n,questionAnswer:""+o});$(this).parent().children("div").css("cursor","default");
$(this).css("opacity","1");$(this).parent().children().unbind();a(this)});$("textarea").keyup(function(){var n=parseInt($(this).parent().attr("id").replace("question-",""));var o=$(this).val();f({questionId:n,questionAnswer:o})});$(".poll-radio").hover(function(){$(this).addClass("checked")},function(){var n=this;
setTimeout(function(){$(n).removeClass("checked")},50)});$("#exit-poll-ok").click(function(){if(!c(this)){d()}return false});var f=function(o){for(var n=0;n<j.length;n++){if(j[n].questionId===o.questionId){j[n].questionAnswer=o.questionAnswer;return}}j.push(o)};var l=function(n){if(j.length!=$(".question").size()){return false
}if(!logoutVariables.userId||!logoutVariables.hash||!e){return false}return true};var g=0.2;var a=function(r){var v=0;var p=false;var t=parseInt($(r).text());var q=parseInt($(r).parent().children(".poll-radio").last().text());var o=$(r).parent().children(".poll-radio").first();var n=$(r).parent().children(".poll-radio").last();
var s=function(){if(v<t){$(r).parent().children(".poll-radio").removeClass("checked");o.css("opacity","0.2");o=o.next();o.prev().css("opacity","0.2");$(r).addClass("checked");$(r).css("opacity","1");v++;setTimeout(function(){s()},50)}else{if(p){$(r).parent().children(".poll-radio").css("opacity","0.2");
$(r).addClass("checked");$(r).css("opacity","1")}else{p=true}}};var u=function(){if(t<q){$(r).parent().children(".poll-radio").removeClass("checked");n.css("opacity","0.2");n=n.prev(".poll-radio");n.next().css("opacity","0.2");$(r).addClass("checked");$(r).css("opacity","1");q--;setTimeout(function(){u()
},50)}else{if(p){$(r).parent().children(".poll-radio").css("opacity","0.2");$(r).addClass("checked");$(r).css("opacity","1")}else{p=true}}};s();u()}});