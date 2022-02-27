<?php

require_once('../../data_classes/server-data.php_data_classes-core.php.php');

$username = $_POST['username'];
$name = $username;
$email = $_POST['email'];
$pw = $_POST['pw'];
$pw_2 = $_POST['pw_2'];
$ntz = $_POST['ntz'];
$hashpw = HoloHashMD5($pw);

$tmp = mysql_query("SELECT id FROM users WHERE username = '".mysql_real_escape_string($username)."' LIMIT 1") or die(mysql_error());
$tmp = mysql_num_rows($tmp);
$first = substr($username, 0, 4);
		
if($username == NULL) {
$error = "<script>alert('Digite um nome de usuário.')</script>"; 
}
else if(strnatcasecmp($first,"MOD-") == false) {
$error = "<script>alert('Nome de usuário inválido.')</script>";
}
else if($tmp > 0){
$error = "<script>alert('Nome já utilizado por outro usuário.')</script>";
}
else if($email == NULL) {
$error = "<script>alert('Digite seu endereço de e-mail')</script>";
}
else if($pw == NULL) {
$error = "<script>alert('Insira sua senha.')</script>";
}
else if($pw_2 == NULL) {
$error = "<script>alert('Insira sua senha.')</script>";
}
else if($pw != $pw_2) {
$error = "<script>alert('As senhas não conferem.')</script>";
}
else if($ntz == NULL) {
$error = "<script>alert('Aceite os termos de uso.')</script>";
}
else { 
mysql_query("UPDATE users SET username='".$name."', mail='".$email."', password='".$hashpw."' WHERE id=".$myrow['id']."");
$error = "<script>alert('Conta criada com sucesso. Bem vindo/a!')</script>";
$_SESSION['username'] = $name;
$_SESSION['password'] = $hashpw;
header("Location: $path/me");
exit;
}			
?>
<?php echo $error; ?>
<meta http-equiv="refresh" content="0;URL=<?php echo $path; ?>/me">
