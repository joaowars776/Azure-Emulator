<?php
require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = Principal;
$pageid = "1";

include('templates/community_subheader.php');
include('templates/community_header.php');
?>
<?php header("Content-Type: text/html; charset=iso-8859-1",true); ?>
<link rel="stylesheet" href="./web-gallery/static/styles/lightweightmepage.css" type="text/css" />
<script src="./web-gallery/static/js/lightweightmepage.js" type="text/javascript"></script>

 <div id="container">
                <div id="content" style="position: relative" class="clearfix">
                    <div id="column1" class="column">
                        <div class="habblet-container ">		
                            <div id="new-personal-info" style="background-image:url(<?php echo $path; ?>/web-gallery/v2/images/personal_info/hotel_views/htlview_jp.png)">
                                <div class="enter-hotel-btn">
                                    
									<?php
$a = mysql_query("SELECT * FROM client_status WHERE hotel='0'");
while($b = mysql_fetch_array($a)){
?>
									    <div class="closed enter-btn">
            <span>Hotel Fechado</span>
        <b></b>
    </div>
									<?php } ?>
									<?php
$a = mysql_query("SELECT * FROM client_status WHERE hotel='1'");
while($b = mysql_fetch_array($a)){
?>
									<div class="open enter-btn">
                                        <a href="/client" target="eac955c8dbc88172421193892a3e98fc7402021a" onclick="HabboClient.openOrFocus(this); return false;">Entre no <?php echo $shortname; ?><i></i></a>
                                        <b></b>
                                    </div>
									<?php } ?>
									
                                </div>
								
	<div id="habbo-plate">
		<a href="profile">
<?php if($myrow['motto'] == "Crikey"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/personal_info/sticker_croco.gif" style="margin-top: 57px"/>
<?php } elseif($myrow['motto'] == "Frank"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/frank_welcome.gif" style="margin-top: 18px"/>
<?php } elseif($myrow['motto'] == "Oops"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/error.gif" style="margin-top: 0px"/>
<?php } elseif($myrow['motto'] == "Skeleton"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/activehomes/habbo_skeleton.gif" style="margin-top: 0px"/>
<?php } elseif($myrow['motto'] == "Batleball"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/personal_info/BBaller.png" style="margin-top: 0px"/>
<?php } elseif($myrow['motto'] == "Afro"){ ?>
			<img alt="<?php echo $name; ?>" src="<?php echo $path; ?>/web-gallery/v2/images/personal_info/AfroKirl.gif" style="margin-top: 0px"/>
<?php } else { ?>
            <img src="<?php echo $avatar; echo $myrow['look']; ?>&size=b&action=stand,wav,crr=667&direction=2&head_direction=2&gesture=sml&size=m" alt="<?php echo $name; ?>">
<?php } ?>
		</a>
	</div>
	
                                <div id="habbo-info">
                                    <div id="motto-container" class="clearfix">			
                                        <strong><?php echo $name; ?>:</strong>
                                        <div>
                                            <span title="What's on your mind today?"><?php echo $myrow['motto']; ?></span>
                                        </div>
                                    </div>
                                </div>
                                <ul id="link-bar" class="clearfix">		
                                    <li class="credits"><font color="#FFFFFF"><?php echo $myrow['credits']; ?></font> Moedas</li>
									<li class="friends"><font color="#FFFFFF"><?php echo $myrow['activity_points']; ?></font> Duckets</li>
                                    <li class="activitypoints"><font color="#FFFFFF"><?php echo $myrow['seasonal_currency']; ?></font> Diamantes</li>
                                </ul>
								
								                                <div id="habbo-feed">
								<?php require_once('./notifications.php'); ?>
								                                </div>
                                <p class="last"></p>
                            </div>
							<div class="habblet-container ">		
<div class="cbb clearfix blue "> 

<h2 class="title">Notícia Destaque</h2> 

<div id="hotcampaigns-habblet-list-container"> 

<ul id="hotcampaigns-habblet-list"> 
<li class="even"> 

<?php
$w = mysql_query("SELECT id,title,longstory,image,link FROM cms_campain ORDER BY id DESC LIMIT 1");
while($z = mysql_fetch_array($w)){
?>
            <div class="hotcampaign-container"> 
                <a href="<?php echo $z['link']; ?>">
				<img src="<?php echo $z['image']; ?>" align="left" alt="<?php echo $z['title']; ?>" /></a> 
                <h3><?php echo $z['title']; ?></h3> 
                <p><?php echo $z['longstory']; ?></p> 
                <p class="link"><a href="<?php echo $z['link']; ?>">Saiba mais &raquo;</a></p> 
            </div>

<?php } ?>
			
</li>
</ul>
</div>
</div>
</div>
                        </div>
                    </div>
<?php
require_once('./data_classes/server-data.php_data_classes-news.php.php');
?>

<div class="habblet-container ">		
<div class="cbb clearfix orange ">
<h2 class="title"><span style="float: left;">Quarto Destaque</span></h2>

			<?php
					$query = mysql_query("SELECT * FROM rooms_data WHERE users_now > '0' ORDER BY users_now DESC LIMIT 1");
						if(mysql_num_rows($query) == 0) {
					echo '<br />     Parece que todos os quartos estão vazios..';
					} 
					while($row = mysql_fetch_assoc($query)) {
					
				?>

<img style="margin-top:5px; margin-left: 10px;position: absolute;" src="<?php echo $path; ?>/room_icon_4.gif" alt="" align="middle"/>
<table style="margin-left: 55px; padding-top: 6px;">
<td>
<span class="room-name"><font size="2"><a><?php echo $row['caption']; ?></a></font></span><br/>
<a><font size="1"><font color="#666666"><?php echo $row['users_now']; ?> usuários dentro dele</font></a>
</td>
</table>

									<?php
					} 
				?>

</div>
</div>

            </div>
        </div>
        <script type="text/javascript">
            HabboView.run();
        </script>
		
        <?php require_once('./templates/community_footer.php'); ?>
    </body>