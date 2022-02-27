<?php
header("Content-Type: image/png");
?>
<?php
header("Content-Type: image/GIF");
?>
<?php
$dbhost = 'localhost'; 
$dbuser = 'root'; 
$dbpass = 'pass'; 
$db = 'kw'; 

mysql_connect($dbhost,$dbuser,$dbpass);   
mysql_select_db($db);   
$action = mysql_real_escape_string( strip_tags( $_GET["action"] ) ); 
$direction = mysql_real_escape_string( strip_tags( $_GET["direction"] ) ); 
$head_direction = mysql_real_escape_string( strip_tags( $_GET["head_direction"] ) ); 
$gesture = mysql_real_escape_string( strip_tags( $_GET["gesture"] ) ); 
$size = mysql_real_escape_string( strip_tags( $_GET["size"] ) ); 
$u = mysql_real_escape_string( strip_tags( $_GET["user"] ) ); 
$userq = mysql_query("SELECT * FROM users WHERE username = '$u' LIMIT 1");
$user = mysql_fetch_array($userq);
$looks = $user['look'];
$look = imagecreatefromgif("http://habbo.com.tr/habbo-imaging/avatarimage?figure='.$looks.'&action=" . $action . "&direction=" . $direction . "&head_direction=" . $head_direction . "&gesture=" . $gesture . "&size=" . $size . "&img_format=gif");


ImageGif ($look);    


// $result = mysql_query($query) or die('Query failed: ' . mysql_error()); //Okay
// if(mysql_num_rows($result)) { //thihi<3
// while($row = mysql_fetch_assoc($result)) { //resultat...
// echo "<img src="http://habbo.com.tr/habbo-imaging/avatarimage?figure=".$row['look']."" alt='".$row['username']."'>";
//        }
// }
?>