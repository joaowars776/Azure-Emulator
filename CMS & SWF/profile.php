<title>Habbluz - Definições</title>
<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-core.php.php');

$pagename = "Meus Detalhes";
$body_id = "profile";
$pageid = "2";

if(isset($_GET['web-profile-tab'])){
	if($_GET['web-profile-tab'] < 2 || $_GET['web-profile-tab'] > 5 || !$_GET['web-profile-tab']){
		header("Location: ".$path."/profile/2");
		$tab = 0;
		exit;
	} else {
		$tab = FilterText($_GET['web-profile-tab']);
	}
} else {
	$tab = "2";
}

if($tab == "2"){
	if(isset($_POST['save'])){
	$motto = $_POST['motto'];
	$chr = chr(1);
	$chr2 = chr(2);
	$motto2 = str_replace($chr, "", $motto);
	$motto3 = str_replace($chr2, "", $motto);

		if(strlen($motto3) > 40){
			$result = "Sua missão é muito larga!";
			$error = "1";
		} else {
			if($_POST['block_newfriends'] == "true"){ $block_newfriends = '0'; }else{ $block_newfriends = '1'; }

			mysql_query("UPDATE users SET motto = '".mysql_real_escape_string($motto3)."', visibility = '".mysql_real_escape_string($_POST['visibility'])."', block_newfriends = '".mysql_real_escape_string($block_newfriends)."', mymusik = '".mysql_real_escape_string($_POST['mymusik'])."' WHERE id = '".$my_id."' LIMIT 1") or die(mysql_error());
			$result = "Seu perfil foi atualizado corretamente!";
		}
	}
} else if($tab == "3"){
	if(isset($_POST['save'])){
	$pass1 = FilterText($_POST['password']);
	$pass1_hash = HoloHashMD5($pass1, $myrow['username']);
	$mail1 = FilterText($_POST['email']);
	$themail = $mail1;
		//checks password --encryption--
		if($pass1_hash == $myrow['password'] ){
		$email_check = preg_match("/^[a-z0-9_\.-]+@([a-z0-9]+([\-]+[a-z0-9]+)*\.)+[a-z]{2,7}$/i", $mail1);
			if($email_check == "1"){
			mysql_query("UPDATE users SET mail = '".$mail1."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());

			$result = "A atualização do e-mail \"".$mail1."\" foi realizada corretamente!";
			} else {
			$result = "A atualização de e-mail está incorreta!";
			$error = "1";
			}
		} else {
		$result = "Seus dados estão incorretos!";
		$error = "1";
		}
	} else {
	$themail = $myrow['mail'];
	}

} else if($tab == "4"){
	if(isset($_POST['save'])){
	$pass1 = FilterText($_POST['password']);
	//Hashes and salts the old password with the user id (in lowercase) --encryption--
	$pass1_hash = HoloHashMD5($pass1, $myrow['name']);
	$newpass = FilterText($_POST['pass']);
	//Hashes and salts the new password with the user id (in lowercase) --encryption--
	$newpass_hash = HoloHashMD5($newpass, $rawname);
	$newpass_conf = FilterText($_POST['confpass']);
		if($pass1_hash == $myrow['password'] ){
			if($newpass == $newpass_conf){
				if(strlen($newpass) < 6){
				$result = "A senha fornecida é muito curta, digite uma de pelo menos 6 carácteres!";
				$error = "1";
				} else {
					if(strlen($newpass) > 51){
					$result = "A senha é muito longa, forneça uma senha com menos de 50 carácteres!";
					$error = "1";
					} else {
					//Atualização sobre encriptação de senha*
					mysql_query("UPDATE users SET password = '".$newpass_hash."' WHERE username = '".$rawname."' and password = '".$rawpass."'") or die(mysql_error());
					$result = "A senha foi alterada. Por favor faça login novamente.";
					}
				}
			} else {
			$result = "As senhas não coincidem.";
			$error = "1";
			}
		} else {
		$result = "Preencha todos os campos requeridos!";
		$error = "1";
		}
	}

}

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div class="content">
<div class="habblet-container" style="float:left; width:210px;">
<div class="cbb settings">
<h2 class="title">Preferências</h2>
<div class="box-content">
<div id="settingsNavigation">
            <ul>
		<?php
		if($tab == "2"){
                echo "<li class='selected'>Minha Missão
                </li>";
		} else {
                echo "<li><a href='".$path."/profile'>Minha Missão
                </li>";
		}
		?>
            </ul>
</div>
</div>
</div>
</div>
<div class="habblet-container " style="float:left; width: 560px;">
<div class="cbb clearfix settings">
<h2 class="title">Mudar Perfil</h2>
<div class="box-content">
<form action="profile" method="post">
<input type="hidden" name="tab" value="" />
<input type="hidden" name="__app_key" value="MyHobba" />
 
 <?php

if(!empty($result)){
	if($error == "1"){
		echo "<div class='rounded rounded-red'>";
	} else {
		echo "<div class='rounded rounded-green'>";
	}

	echo $result . "<br />
	</div><br />";
}

$user_sql = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'");
$user_row = mysql_fetch_assoc($user_sql);

?>
 
<h3><b>Seu estado</b></h3>
<p>Seu estado é o que os outros Habbluzes irão ver no seu perfil dentro do Habbluz.</p>
<p><label>Estado: <input type="text" name="motto" size="32" maxlength="32" value="<?php echo HoloText($user_row['motto']); ?>" id="avatarmotto" />
</label></p>
<div style="border-top: 1px dotted; padding-top: 8px; "></div>
 
<h3>Pedidos</h3>
<p>

<label>
<input name="block_newfriends" <?php if($user_row['block_newfriends'] == 0){ ?>checked="checked"<?php } ?> value="true" type="checkbox">
Pedido de amizades habilitado (se não quiser receber pedidos de amizade, desabilite esta opção)
</label></p>
			
								<div class="settings-buttons">
<input type="submit" value="Salvar preferências" name="save" class="submit" />
								</div>
								
							</form>
											</div>
				</div>
			</div>
		</div>


		<script type="text/javascript">
			HabboView.run();
		</script>
<?php require_once('./templates/community_footer.php'); ?>
		</div>
	</div>
</div>


</body>