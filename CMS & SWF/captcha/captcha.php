<?php
require('./php-captcha.inc.php');
$aFonts = array('./monofont.ttf');
$oPhpCaptcha = new PhpCaptcha($aFonts, 200, 60);
$oPhpCaptcha->SetWidth(200);
$oPhpCaptcha->SetHeight(60);
$oPhpCaptcha->SetNumChars(6);
$oPhpCaptcha->SetCharSet('a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z');
$oPhpCaptcha->SetNumLines(70);
$oPhpCaptcha->DisplayShadow(false);
$oPhpCaptcha->SetMaxFontSize(50);
$oPhpCaptcha->SetMinFontSize(40);
$oPhpCaptcha->UseColour(false);
$oPhpCaptcha->Create();
?>