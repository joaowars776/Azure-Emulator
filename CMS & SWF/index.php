<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php'); 
if (isset($_SESSION['username'])) {
header("location: $path/me");
}
$ip_check = mysql_query("SELECT ip_last,username,id FROM users WHERE ip_last = '".$remote_ip."'");
	$page = HoloText($_GET['p']);

if(isset($_POST['credentials_username']) && isset($_POST['credentials_password']))
{
	if(empty($_POST['credentials_username']))
	{
		$login_fehler = "Entre com seu nome e senha para login.";
	}
	elseif(empty($_POST['credentials_password']))
	{
		$login_fehler = "Digite sua senha.";
	}
	
	else
	{
	
		if(isset($_COOKIE['password']))
		{
			$pwd = HoloHashMD5($_COOKIE['password']);
		}
		else
		{
			$pwd = HoloHashMD5($_POST['credentials_password']);
		}
		$userq = mysql_query("SELECT username, password FROM users WHERE username = '".HoloText($_POST['credentials_username'])."' AND password = '".$pwd."' LIMIT 1");
		if(mysql_num_rows($userq) == 1)
		{
			$user = mysql_fetch_assoc($userq);
			
			$banq = mysql_query("SELECT id, value, reason, expire FROM bans WHERE (value = '".$user['username']."' OR value = '".$_SERVER['REMOTE_ADDR']."') AND expire > '".time()."' LIMIT 1");
			if(mysql_num_rows($banq) == 1)
			{
				$ban = mysql_fetch_assoc($banq);
			
				$login_fehler = "Você foi banido por ".$ban['reason']." atÃ© ".date("d/m/Y H:i:s", $ban['expire']);
			}
			else
			{
			mysql_query("UPDATE cms_settings SET logins = logins +1");
			
				if(isset($_POST['_login_remember_me']))
				{
					$_COOKIE['username'] = $user['username'];
					$_COOKIE['password'] = $user['password'];
				}
				
				$_SESSION['username'] = $user['username'];
				$_SESSION['password'] = $user['password'];
				
				if (isset($page)) { 
										header("location: $path/$page");
										}
										else {
										header("location: $path/me");
										}
										
			}
		}
		else
		{
			$login_fehler = "Sua senha e seu email não conferem.";
		}
	}
}
require_once('./habblet/login/ajax.php');
require_once('./habblet/ajax/mobile_detector.php'); ?>


