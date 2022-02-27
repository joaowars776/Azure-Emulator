<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

if(Loged_In == FALSE)
{
    header("Location: index.php");
    exit;
}

if(isset($_GET['m'])) {
	if(!empty($_GET['m'])) {
		$m = mysql_real_escape_string($_GET['m']);

        if($m == '903ucd29n84377384tf6cbgauydgbu73w4i') {
        	mysql_query("UPDATE users SET activity_points = Diamantes + 10 WHERE id = '". $myrow['id'] ."'");
        	header("Location: badges.php");
        	exit;
        } 
        elseif($m == 'f3ofn87ched9cfn8wcro87fuhwnnc4rf') {
        	mysql_query("UPDATE users SET activity_points = Diamantes + 25 WHERE id = '". $myrow['id'] ."'");
        	header("Location: badges.php");
        	exit;
        } 
        elseif($m == 'dnhifx34xi2w47f6igw4rxnfgnwi2fon') { 
            mysql_query("UPDATE users SET activity_points = Diamantes + 60 WHERE id = '". $myrow['id'] ."'");
            header("Location: badges.php");
            exit;
        } 
        elseif($m == 'dh38endh2387n4gxhqinndu7y348n7nd') { 
            mysql_query("UPDATE users SET activity_points = Diamantes + 100 WHERE id = '". $myrow['id'] ."'");
            header("Location: badges.php");
            exit;
        } else {
        	echo 'Algo deu errado! Tente mais tarde..';
        }

    } else { 
    	header("Location: index.php");
    }
} elseif(isset($_GET['b']) && !empty($_GET['b'])) {
    $b = mysql_real_escape_string($_GET['b']);

    $b_info_b = mysql_query("SELECT * FROM cms_badges WHERE badge_id = '". $b ."'");
    $b_info = mysql_fetch_assoc($b_info_b);

    if(mysql_num_rows($b_info_b) > 0) {
        if($myrow['activity_points'] >= $b_info['cost']) {
            if($b_info['b_limit'] > 0) {

                $sql_b = mysql_query("INSERT INTO users_badges (user_id, badge_id, badge_slot) VALUES ('". $myrow['id'] ."', '". $b_info['badge_id'] ."', '0')");
                $sql_b_c = mysql_query("UPDATE users SET activity_points = activity_points - '". $b_info['cost'] ."' WHERE id = '". $myrow['id'] ."'");
                $sql_b_l = mysql_query("UPDATE cms_badges SET b_limit = b_limit - 1 WHERE id = '". $b_info['id'] ."'");
        
                if($sql_b && $sql_b_c && $sql_b_l) {
                    header("Location: badges.php");
                    exit;
                } else {
                    $error = '<div id="error">Algo deu errado! Tente mais tarde..</div>';
                }   
            } else {
                header("Location: badges.php");
                exit;
            }   
        } else { 
            header("Location: badges.php");
            exit;
        }
    } else {
        header("Location: badges.php");
        exit;
    }     
} else {
    header("Location: badges.php");
    exit;
}
?>