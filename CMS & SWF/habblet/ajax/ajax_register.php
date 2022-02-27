<?php
require_once('../../data_classes/server-data.php_data_classes-core.php.php');
header('Content-type: application/json');

if($_POST)
{
	$failure = false;
	$registrationErrors = array();
	
	// USERNAME
	
	if(empty($_POST['registrationBean_username']))
	{
		$failure = true;
		$registrationErrors['registration_username'] = "Escolha o seu nome ".$sitename.".";
	}
	elseif(!preg_match("/^[A-Z0-9=?!@:.-]{2,15}$/i", $_POST['registrationBean_username']))
	{
		$failure = true;
		$registrationErrors['registration_username'] = "Nome inválido.";
	}
	elseif(mysql_num_rows(mysql_query("SELECT id FROM users WHERE username = '".HoloText($_POST['registrationBean_username'])."' LIMIT 1")) == 1)
	{
		$failure = true;
		$registrationErrors['registration_username'] = "Este nome já é utilizado por outro usuário.";
	}
	
	// E-MAIL 
	
	if(empty($_POST['registrationBean_email']))
	{
		$failure = true;
		$registrationErrors['registration_email'] = "Digite o seu endereço de e-mail.";
	}
	elseif(!preg_match("/^[A-Z0-9._-]{2,}+@[A-Z0-9._-]{2,}\.[A-Z0-9._-]{2,}$/i", $_POST['registrationBean_email']))
	{
		$failure = true;
		$registrationErrors['registration_email'] = "Por favor, insira um endereço de e-mail válido.";
	}
	
	// PASSWORT
	
	if(empty($_POST['registrationBean_password']))
	{
		$failure = true;
		$registrationErrors['registration_password'] = "Digite a sua senha.";
	}
	elseif(strlen($_POST['registrationBean_password']) < 6)
	{
		$failure = true;
		$registrationErrors['registration_password'] = "A sua senha deve ter pelo menos 6 caracteres";
	}
	
	elseif($_POST['registrationBean_password'] != $_POST['registrationBean_password2'])
	{
	$failure = true;
	$registrationErrors['registration_password'] = "As senhas não coincidem.";
	}
	
	// PASSWORT CHECK
	
	
	if(empty($_POST['registrationBean_password2']))
	{
		$failure = true;
		$registrationErrors['registration_password2'] = "Por favor, reescreva a senha.";
	}
	elseif(strlen($_POST['registrationBean_password2']) < 6)
	{
		$failure = true;
		$registrationErrors['registration_password2'] = "A sua senha deve ter pelo menos 6 caracteres";
	}
	
	elseif($_POST['registrationBean_password2'] != $_POST['registrationBean_password'])
	{
	$failure = true;
	$registrationErrors['registration_password2'] = "As senhas não coincidem.";
	}
	
	// AGB
	
	//if(!isset($_POST['tos']))
	//{
//		$failure = true;
//		$registrationErrors['registration_termsofservice'] = "Bitte Akzeptiere die AGB!";
//	}
	

	// GEBURTSDATUM
	
	
	
	if(empty($_POST['registrationBean_day']) || empty($_POST['registrationBean_month']) || empty($_POST['registrationBean_year']))
	{
		$failure = true;
		$registrationErrors['registration_birthday_format'] = "Escolha uma data de nascimento válida.";
	}
	elseif(!checkdate(intval($_POST['registrationBean_month']), intval($_POST['registrationBean_day']), intval($_POST['registrationBean_year'])))
	{
		$failure = true;
		$registrationErrors['registration_birthday_format'] = "Escolha uma data de nascimento válida.";
	}
	
	
	if(!$failure)
	{
	
	$username = HoloText($_POST['registrationBean_username']);
	$email = HoloText($_POST['registrationBean_email']);
	$password = HoloText($_POST['registrationBean_password']);
	$password2 = HoloText($_POST['registrationBean_password2']);
	$tag = HoloText($_POST['registrationBean_day']);
	$monat = HoloText($_POST['registrationBean_month']);
	$jahr = HoloText($_POST['registrationBean_year']);
	$sexe = HoloText($_POST['registrationBean_gender']);
			if($sexe == "M") {
				$look = $register['look_m'];
			} else {
				$look = $register['look_f'];
			}

mysql_query("INSERT INTO users (online,vip_points,vip,username,password,mail,auth_ticket,rank,look,gender,motto,last_online,account_created,ip_last,ip_reg)
			VALUES ('0','0','0','".$username."','".HoloHashMD5($password)."','".$email."','','1','hd-180-10.ch-210-1408.lg-270-1408.sh-290-1408.he-1606-1408','M','Mude sua missão ..','".time()."','".time()."','".$remote_ip."','".$remote_ip."')") or die(mysql_error());
		
		$_SESSION['username'] = $username;
		$_SESSION['password'] = HoloHashMD5($password);
		
		$userdata2 = mysql_query("SELECT * FROM users WHERE username = '".$_SESSION['username']."'");
$userdata = mysql_fetch_assoc($userdata2);
mysql_query("INSERT INTO `user_info` (user_id,reg_timestamp) VALUES ('".$userdata['id']."','".time()."')");
mysql_query("INSERT INTO `user_stats` (id) VALUES ('".$userdata['id']."')");

		echo json_encode(array("registrationCompletionRedirectUrl" => $path."/client"));
	}
	elseif(!empty($registrationErrors))
	{
		$encoded = json_encode(array("registrationErrors" => $registrationErrors, "registrationMessages" => array()));
		echo $encoded;
	}
}
?>
