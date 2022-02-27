<?php
#APRIMORAMENTOS POR @RTIAG0
define("IN_HOLOCMS", TRUE);
@session_start();

// #########################################################################
// CONEXÃO COM O BANCO DE DADOS
// #########################################################################

@require_once('server-data.php_data_classes-config.php.php');
mysql_connect("$MySQLhostname", "$MySQLusername", "$MySQLpassword") or die("Erro em conexão com o MySQL"); 
mysql_select_db("$MySQLdb") or die("Banco de dados inexistente");
error_reporting(E_ALL & ~E_NOTICE & ~E_DEPRECATED & ~E_WARNING);
error_reporting(0);

// #########################################################################
// CONFIGURAÇÕES
// #########################################################################

$cms_name = mysql_fetch_assoc($cms_name = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_name'"));
$cms_url = $hotel_url;

foreach($_GET As $name=>$value) {
	$_GET[$name]=mysql_real_escape_string($value); 
}
foreach($_POST as $name => $value) {
	$_POST[$name] = mysql_real_escape_string($value);
}

$remote_ip = $_SERVER['REMOTE_ADDR'];
$sitename = "".$cms_name['value']."";
$shortname = "".$cms_name['value']."";

if(@ini_get('date.timezone') == null && function_exists("date_default_timezone_get")){ @date_default_timezone_set("Europe/Madrid"); }

$H = date('H');
$i = date('i');
$s = date('s');
$m = date('m');
$d = date('d');
$Y = date('Y');
$j = date('j');
$n = date('n');
$today = $d;
$month = $m;
$year = $Y;
$getmoney_date = date('d/m/Y',mktime($m,$d,$Y));
$birthday_date = date('d/m', mktime($m,$d));
$date_normal = date('d/m/Y',mktime($m,$d,$Y));
$date_full = date('d/m/Y H:i:s',mktime($H,$i,$s,$m,$d,$Y));
$path = $hotel_url;
$adminpath = mysql_real_escape_string("".$path."/theallseeingeye/hotel/de/housekeeping"); //*Painel de Controle*//
$clientpath = $hotel_url;
$cimagesurl = $cimages_url;
$badgesurl = "/swf/c_images/album1584/";
$hash_secret = "xCg532%@%gdvf^5DGaa6&*rFTfg^FD4\$OIFThrR_gh(ugf*/";

@require_once('server-data.php_data_classes-config.php.php'); //*Arquivo de configuração*//
$maintenance = mysql_num_rows($maintenance = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_maintenance' AND value = '1'"));

$server = mysql_fetch_assoc($server_status = mysql_query("SELECT * FROM server_status"));
$online_count = $server['users_online'];

if(isset($_POST) || isset($_GET) || isset($_REQUEST) || isset($_COOKIE)){
		foreach($_POST as $key => $p)
		{
		$_POST[$key] = htmlentities($p);
			$_POST[$key] = mysql_real_escape_string($p);
			$_POST[$key] = html_entity_decode($p);
		}
		
		foreach($_GET as $key => $g)
		{	
			$_GET[$key] = mysql_real_escape_string($g);
		}
	foreach($_COOKIE as $key => $s)
		{	
			$COOKIE[$key] = mysql_real_escape_string($s);
		}
		foreach($_REQUEST as $key => $k)
		{
			$_REQUEST[$key] = mysql_real_escape_string($k);
		}
	}
if(isset($_GET)){
		
		foreach($_GET as $key => $f)
		{	
			$_GET[$key] = strip_tags(mysql_real_escape_string(htmlentities($f)));
		}
	}

// #########################################################################
// FUNÇÕES ADICIONAIS NOVAS
// #########################################################################

function IsOnline($id){
	if($server == '1') {
			$num = mysql_num_rows(mysql_query("SELECT * FROM user_online WHERE userid = '".$id."'"));
			if($num > 0){
				return true;
			} else {
				return false;
			}
	}else {
	$data = mysql_fetch_assoc(mysql_query("SELECT * FROM users WHERE id = '".$id."' LIMIT 1"));
	if($data['online'] == '1') {
		return true;
	}
	else {
		return false;
	}
	}
}

// #########################################################################
// ENCRIPTAÇÃO DE SENHA
// #########################################################################

function HoloHash($password){
	$hash_secret = "xCg532%@%gdvf^5DGaa6&*rFTfg^FD4\$OIFThrR_gh(ugf*/";
	$string = sha1($password.($hash_secret));
	return $string;
}

function HoloHashMD5($password){
	$hash_secret = "xCg532%@%gdvf^5DGaa6&*rFTfg^FD4\$OIFThrR_gh(ugf*/";
	$string = md5($password.($hash_secret));
	return $string;
}

function tirartags($palavra){
    $palavra = str_replace('innerHTML', '', $palavra);
    $palavra = str_replace('alert(', '', $palavra);
    $palavra = str_replace('documentElement', '', $palavra);
	return $palavra;
}

// #########################################################################
// USUÁRIOS BANIDOS
// #########################################################################

if(empty($_SESSION['username']) && @$_COOKIE['remember'] == 'remember'){

	$cname = FilterText($_COOKIE['rusername']);
	$cpass_hash = $_COOKIE['rpassword'];

	$csql = mysql_query("SELECT password,id FROM users WHERE username = '".$cname."' LIMIT 1") or die(mysql_error());
	$cnum = mysql_num_rows($csql);

		if($cnum < 1){
			setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
		} else {

			$crow = mysql_fetch_assoc($csql);
			$correct_pass = $crow['password'];

			if($cpass_hash == $correct_pass){
				$_SESSION['username'] = $cname;
				$_SESSION['password'] = $crow['password'];
				$sql3 = mysql_query("UPDATE users SET ip_last = '".$remote_ip."' WHERE username = '".$cname."'");
				header("location: me"); exit;
			} else {

				setcookie("remember", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
				setcookie("rusername", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
				setcookie("rpassword", "", time()-60*60*24*100, "/"); setcookie("cookpass", "", time()-60*60*24*100, "/");
			}
		}
}

// #########################################################################
// IS-EVEN FUNKTION
// #########################################################################

function IsEven($intNumber)
{
	if($intNumber % 2 == 0){
		return true;
	} else {
		return false;
	}
}

// #########################################################################
// SMILES EM BBCODE
// #########################################################################
function SearchMotto($mision){
$mision = str_replace("¡", "&iexcl;", $mision);
}
function bbcode_format($str){

	$str = str_replace(":)", " <img src='./web-gallery/smilies/smile.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";)", " <img src='./web-gallery/smilies/wink.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":P", " <img src='./web-gallery/smilies/tongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";P", " <img src='./web-gallery/smilies/winktongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":p", " <img src='./web-gallery/smilies/tongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(";p", " <img src='./web-gallery/smilies/winktongue.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace("(L)", " <img src='./web-gallery/smilies/heart.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace("(l)", " <img src='./web-gallery/smilies/heart.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":o", " <img src='./web-gallery/smilies/shocked.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace(":O", " <img src='./web-gallery/smilies/shocked.gif' alt='Smiley' title='Smiley' border='0'> ", $str);
	$str = str_replace("\n", chr(10), $str);

        $simple_search = array(
                                '/\[b\](.*?)\[\/b\]/is',
                                '/\[i\](.*?)\[\/i\]/is',
                                '/\[u\](.*?)\[\/u\]/is',
                                '/\[s\](.*?)\[\/s\]/is',
                                '/\[quote\](.*?)\[\/quote\]/is',
                                '/\[link\=(.*?)\](.*?)\[\/link\]/is',
                                '/\[url\=(.*?)\](.*?)\[\/url\]/is',
                                '/\[color\=(.*?)\](.*?)\[\/color\]/is',
                                '/\[size=small\](.*?)\[\/size\]/is',
                                '/\[size=large\](.*?)\[\/size\]/is',
                                '/\[code\](.*?)\[\/code\]/is',
                                '/\[Habbo\=(.*?)\](.*?)\[\/Habbo\]/is',
                                '/\[room\=(.*?)\](.*?)\[\/room\]/is',
                                '/\[group\=(.*?)\](.*?)\[\/group\]/is'
	);

        $simple_replace = array(
                                '<strong>$1</strong>',
                                '<em>$1</em>',
                                '<u>$1</u>',
                                '<s>$1</s>',
                                "<div class='bbcode-quote'>$1</div>",
                                "<a href='$1'>$2</a>",
                                "<a href='$1'>$2</a>",
                                "<font color='$1'>$2</font>",
                                "<font size='1'>$1</font>",
                                "<font size='3'>$1</font>",
                                '<pre>$1</pre>',
                                "<a href='./user_profile.php?id=$1'>$2</a>",
                                "<a onclick=\"roomForward(this, '$1', 'private'); return false;\" target=\"client\" href=\"./client.php?forwardId=2&roomId=$1\">$2</a>",
                                "<a href='./group_profile.php?id=$1'>$2</a>"
	);

        $str = preg_replace ($simple_search, $simple_replace, $str);

        return $str;
}

// #########################################################################
// SSO TICKET PARA BUTERFLYEMULADOR / BUTTERSTORM
// #########################################################################

function GenerateTicket(){

	$data = "ST-";

	for ($i=1; $i<=6; $i++){
		$data = $data . rand(0,9);
	}

	$data = $data . "-";

	for ($i=1; $i<=20; $i++){
		$data = $data . rand(0,9);
	}

	$data = $data . "";
	$data = $data . rand(0,5);

	return $data;
}

// #########################################################################

if(!empty($_SESSION['username'])){

	$rawname = $_SESSION['username'];
	$rawpass = $_SESSION['password'];

	$usersql = mysql_query("SELECT * FROM users WHERE username = '".$rawname."' AND password = '".$rawpass."' LIMIT 1");
	$myrow = mysql_fetch_assoc($usersql);


	$password_correct = mysql_num_rows($usersql);

	$my_id = $myrow['id'];
	$user_rank = $myrow['rank'];
	$user_time = $myrow['time'];
	$ban = mysql_query("SELECT * FROM bans WHERE value = '".$myrow['username']."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1");
	$bancheck = mysql_num_rows($ban);

	if($myrow['ip_reg'] == "0"){
		mysql_query("UPDATE users SET ip_reg = '".$remote_ip."' WHERE id = '".$myrow['id']."'");

	}elseif($password_correct !== 1){

	session_destroy();
	header("location: ".$path."1");
	exit;

	}elseif($bancheck > 0){

	$bandata = mysql_fetch_assoc($ban);

	$timestamp = time();
	if($bandata['expire'] > $timestamp){
	$login_error = "Du bist gebannt! Der Grund für deinen Bann lautet \"".$bandata['reason']."\" und dauert bis ".date('d.m.Y - H:i:s', $bandata['expire'])."";			
	include('logout.php');
	session_destroy(); exit;

	} else{ 
		mysql_query("DELETE FROM bans WHERE value = '".$name."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1"); }  
	}

	$logged_in = true;
	$name = HoloText($myrow['username']);

	} else {

	$user_rank = 0;
	$name = "No-Name";
	$my_id = "No-ID";
	$myticket = "ST-No-Name-Habbore-fe";
	$logged_in = false;

}

// #########################################################################
// HC CHECK
// #########################################################################

	$hc_a = mysql_query("SELECT * FROM user_subscriptions WHERE user_id = '".$my_id."' and timestamp_expire > '".time()."'");
	$hc = mysql_num_rows($hc_a);

	function getHCDays($my_id){

		$sql = mysql_query("SELECT timestamp_activated,timestamp_expire FROM user_subscriptions WHERE user_id = '".$my_id."' LIMIT 1") or die(mysql_error());
		
		if (mysql_num_rows($sql) == 0){
			return 0;
		}
		
		$data = mysql_fetch_assoc($sql);
		$diff = $data['timestamp_expire'] - time();
		
		if ($diff <= 0){
			return 0;
		}
		
		return ceil($diff / 86400);
	}

// #########################################################################
// MANUTENÇÃO
// #########################################################################

if($user_rank > 8){

	if(session_is_registered(hkusername) && session_is_registered(hkpassword)){ 
		$rank['iAdmin'] = "1";
	} else {
		$rank['iAdmin'] = "0"; 
	}

} else { 
	$rank['iAdmin'] = "0";
}

if($maintenance == '1' && !$is_maintenance && $rank['iAdmin'] < 1){
	header("Location: ".$path."/maintenance");
	exit;
} elseif($rank['iAdmin'] == 1 && $config['variable'] == "cms_maintenance" && $config['value'] == '1'){
	$notify_maintenance = true;
}

// #########################################################################

function GetUserBadge($strName){ // supports user IDs also/ supports user IDs also/ supports user IDs also

	if(is_numeric($strName)){
		$check = mysql_query("SELECT id FROM users WHERE id = '".$strName."' AND badge_status = '1' LIMIT 1") or die(mysql_error());
	} else {
		$check = mysql_query("SELECT id FROM users WHERE username = '".FilterText($strName)."' AND badge_status = '1' LIMIT 1") or die(mysql_error());
	}

	$exists = mysql_num_rows($check);

		if($exists > 0){
			$usrrow = mysql_fetch_assoc($check);
			$check = mysql_query("SELECT * FROM user_badges WHERE user_id = '".$usrrow['id']."' AND badge_slot = '1' LIMIT 1") or die(mysql_error());
			$hasbadge = mysql_num_rows($check);
			if($hasbadge > 0){
				$badgerow = mysql_fetch_assoc($check);
				return $badgerow['badge_id'];
			} else {
				return false;
			}
		} else {
			return false;
		}
}

function GetUserGroup($my_id){
$check = mysql_query("SELECT id_group FROM group_members WHERE id_user = '".$my_id."' AND is_current = '1' LIMIT 1") or die(mysql_error());
$has_fave = mysql_num_rows($check);

	if($has_fave > 0){

		$row = mysql_fetch_assoc($check);
		$groupid = $row['id_group'];

		return $groupid;

	} else {

		return false;

	}
}

function GetUserGroupBadge($my_id){
$check = mysql_query("SELECT id_group FROM group_members WHERE id_user = '".$my_id."' AND is_current = '1' LIMIT 1") or die(mysql_error());
$has_badge = mysql_num_rows($check);

	if($has_badge > 0){

		$row = mysql_fetch_assoc($check);
		$groupid = $row['id_group'];

		$check = mysql_query("SELECT badge FROM group_details WHERE id = '".$groupid."' LIMIT 1") or die(mysql_error());

		$row = mysql_fetch_assoc($check);
		$badge = $row['badge'];

		return $badge;

	} else {

		return false;

	}
}

function IsUserBanned($name){

	$check = mysql_query("SELECT * FROM bans WHERE value = '".$my_id."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip'") or die(mysql_error());
	$is_banned = mysql_num_rows($check);

	if($is_banned > 0){
		$bandata = mysql_fetch_assoc($check);
		$reason = $bandata['reason'];
		$expire = $bandata['expire'];

		$stamp_now = time();

		if($stamp_now < $bandata['expire']){
			return true;
		} else { // ban expired
			mysql_query("DELETE FROM bans WHERE value = '".$my_id."' AND bantype = 'user' or value = '".$remote_ip."' AND bantype = 'ip' LIMIT 1") or die(mysql_error());
			return false;
		}
	} else {
		return false;
	}
}

// #########################################################################

function mysql_evaluate($query, $default_value="undefined") {
	$result = mysql_query($query) or die(mysql_error());

	if(mysql_num_rows($result) < 1){
		return $default_value;
	} else {
		return mysql_result($result, 0);
	}
}

// #########################################################################

function FilterText($str, $advanced=false) {
	if($advanced == true){ return mysql_real_escape_string($str); }
	$str = mysql_real_escape_string(htmlspecialchars($str));
	return $str;
}


function HoloText($str, $advanced=false, $bbcode=false) {
	if($advanced == true){ return stripslashes($str); }
	$str = nl2br(htmlspecialchars($str));
	if($bbcode == true){$str = bbcode_format($str); }
	return $str;
}


?>
