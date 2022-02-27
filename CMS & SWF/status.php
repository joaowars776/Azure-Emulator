<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Status";
$pageid = "5";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php');

?>
<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div style="margin-bottom:5px;margin-top:5px;margin-left:20px;">
<script async src="//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js"></script>
 
<ins class="adsbygoogle" style="display:inline-block;width:728px;height:90px" data-ad-client="ca-pub-5602546026435487" data-ad-slot="8865148626"></ins>
<script>
                (adsbygoogle = window.adsbygoogle || []).push({});
            </script>
</div>
<style>.recommendedrooms-lite-habblet-list-container ul.habblet-list li{padding:5px 15px;min-height:1%;}.recommendedrooms-lite-habblet-list-container .enter-room-link:hover{text-decoration:none;}.recommendedrooms-lite-habblet-list-container .enter-room-link:hover .room-enter,.recommendedrooms-lite-habblet-list-container .enter-room-link:hover .room-name{text-decoration:underline;}.enter-room-link{display:block;background:transparent url(../images/rooms/room_icon_1.gif) no-repeat 0 1px;padding:5px 0 5px 42px;cursor:pointer;}.recommendedrooms-lite-habblet-list-container .enter-room-link .room-enter{float:right;padding-right:26px;margin:10px 0 10px 10px;color:#4AB501;background:transparent url(../images/info_icons.png) no-repeat 100% -800px;line-height:16px;cursor:pointer;}.recommendedrooms-lite-habblet-list-container .enter-room-link .room-description,.recommendedrooms-lite-habblet-list-container .room-owner{display:block;font-size:11px;}#column2 .room-owner{display:none;}#column2 .room-description{height:1.2em;overflow:hidden;}#column2 .recommendedrooms-lite-habblet-list-container ul.habblet-list li{padding:2px 15px;}.recommendedrooms-lite-habblet-list-container .enter-room-link .room-name,.room-summaryinfo .room-toggle-fullinfo{color:#000;}.recommendedrooms-lite-habblet-list-container .enter-room-link .room-name{font-size:12px;}.recommendedrooms-lite-habblet-list-container .enter-hotel-link{float:none;display:block;height:16px;padding:0;margin:4px 0 3px 0;width:16px;}ul.room-more-data{margin-top:0pt;}.recommendedrooms-habblet-list-container ul.habblet-list li{padding-left:10px;}a.room-toggle-more-data{background:transparent url(../images/more_arrows.png) no-repeat 100% -189px;float:left;margin-top:7px;padding-right:14px;margin:10px 0 11px 10px;}a.room-toggle-more-data.less{background:transparent url(../images/more_arrows.png) no-repeat 100% -237px;}.room-description,.room-owner{color:#666666;}div.room-description{word-break:break-all;}div.room-icon{background-image:url(../images/info_icons.png);margin:-31px 0pt 0pt 85px;width:16px;height:16px;}div.room-password-protected{background-position:0 -624px;}div.room-locked{background-position:0 -576px;}#roomlink-habblet-container{background-color:#e1d5b7;height:1%;}#roomlink-habblet-container div.roominfo,#roomlink-habblet-container div.room-description{padding:0;width:125px;}#roomlink-habblet-container div.room-name-label{font-weight:bold;}</style>
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix green ">
<h2 class="title"><span style="float: left;">Atualmente no Hotel</span></h2>
<div style="padding:5px;">
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM users"); ?></strong> Usuários registrados.<br/>
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM users WHERE rank ='2'"); ?></strong> Usuários VIP.<br/>
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM rooms_data"); ?></strong> Quartos criados.<br/>
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM items_rooms"); ?></strong> Mobis comprados.<br/>
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM bans"); ?></strong> Usuários banidos.<br/>
 &raquo; Temos <strong><?php echo mysql_evaluate("SELECT COUNT(*) FROM groups_data"); ?></strong> Grupos criados.<br/>
</div>
</div>
</div>
</div>

<div id="column2" class="column">
<div class="habblet-container ">
<div class="cbb clearfix orange ">
<h2 class="title">
<span style="float: left;">Últimas atualizações</span>
</h2>
<div style="padding:5px;">

<?php
$e = mysql_query("SELECT id,categoria,motivo FROM cms_statslogs ORDER BY id DESC LIMIT 3");
while($f = mysql_fetch_array($e)){
?>

 &raquo; <strong><?php echo $f['categoria'] ?></strong> <?php echo $f['motivo'] ?>
<br/>
<hr>

<?php } ?>

</div>
</div>
</div>
</div>

<script type="text/javascript">
            document.observe("dom:loaded", function() { PromoSlideShow.init(); });
        </script>
<script type="text/javascript">
			HabboView.run();
		</script>
</div>
<?php require_once('./templates/community_footer.php'); ?>
</div>
</div>
</div>
</body>