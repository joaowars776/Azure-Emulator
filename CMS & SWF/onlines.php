<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = Onlines;
$pageid = "3";

?>
<html xmlns="http://www.w3.org/1999/xhtml">
<head> 
    <?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
	<title><?php echo $shortname; ?>: Onlines</title> 
	<link rel="stylesheet" type="text/css" href="http://fonts.googleapis.com/css?family=Ubuntu:regular,bold&subset=Latin">
	<link rel="stylesheet" href="/app/tpl/skins/Habbo/styles/style.css">
	<link rel="stylesheet" href="/app/tpl/skins/Habbo/styles/online.css">
	<link rel="shortcut icon" href="/images/web-gallery/v2/favicon.ico" type="image/vnd.microsoft.icon" />
	<script type="text/javascript" src="/app/tpl/skins/Habbo/js/basic.js"></script> 
	<script type="text/javascript" src="/app/tpl/skins/Habbo/js/javascript.js"></script> 
	
</head>

<body oncontextmenu="return false" onselectstart="return false" ondragstart='return false'>
	<div class="container"> 
				
		<div class="header">
			
			<a href="/">
			<div class="logo">
			<img src="/app/tpl/skins/Habbo/images/logo.png" class="animated tada" style="float: left; margin-top: 20px; padding-left: 10px;" />
			</div>
			</a>
			
			<div id="onlinestats">
				<div style="float: left; margin-right: 5px; border-right: 2px solid rgba(204, 204, 204, 0.61); padding-right: 10px;">
					<img src="/app/tpl/skins/Habbo/images/icons/register.gif" style="float:left;margin-top: -1px;margin-right: 7px;"><b><?php echo mysql_evaluate("SELECT COUNT(*) FROM users"); ?></b> Registrados
				</div>
					
				<div style="margin-left:5px;float:right;">
					<img src="/app/tpl/skins/Habbo/images/icons/online.gif" style="float:left;margin-top: -1px;margin-right: 7px;"><b><?php echo $online_count; ?></b> Onlines
				</div>
			</div>
			
		 </div>
		 
		 <?php require_once('./menu/navigator.php'); ?>
		 
		 		<div id="sub_navigation">
			<span class="submenu" id="sub1" style="display: inline;">  
				<div class="item"><a><?php echo $name; ?></a></div><div class="item"><div class="item"><a href="/profile">Ajustes</a></div><a href="/onlines">Onlines</a></div><div class="item"></div>
				</span>  
		</div>	

		<div class="container-left">
			<div class="contentTitle red">O que é isso?</div>
			<div class="box"> 
				<div class="padding">
					Isso é uma página aonde você pode ver quem está jogando o hotel agorinha mesmo! Assim você fica sabendo que aquela pessoa está jogando o hotel!<p></p> Agora temos <strong>(<?php echo $online_count; ?>)</strong> jogadores onlines. 
				</div>
				<div style="clear:both"></div>
			</div>
		</div>
		
		<div class="container-right">
			<div class="contentTitle blue">Usuários Ativos
			</div>
			<div class="box"> 
				<div class="padding">
					  
<?php
$e = mysql_query("SELECT username,look,online,id FROM users WHERE online='1' ORDER BY ID ASC");
while($f = mysql_fetch_array($e)){
?>
					  
							<a href='#' class='zebra_tips1' style='float: left;' title='<?php echo $f['username'] ?>'>
							<div style='width: 73px; height: 73px; float: left; margin: 0px 0.2px 0px 0px;'>
							<div class='user-icon-grey' style='box-shadow: 3px 3px 8px #A2A2A2;'>
							<div class='user-icon-avatar'>
							<img border='0' src='<?php echo $avatar; echo $f['look']; ?>&direction=2&head_direction=3&gesture=sml&size=m'> 
							</div>
							</div>
							</div>
							</a>
							
<?php } ?>
							
			    </div>
				<div style="clear:both"></div>
			</div>
		</div>
		
		<div style="clear:both"></div>
<?php require_once('./menu/footer.php'); ?>
</body>
</html>