<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');


mysql_query("UPDATE users SET auth_ticket = '', auth_ticket = '".GenerateTicket()."', ip_last = '', ip_last = '".$remote_ip."' WHERE id = '".$my_id."'") or die(mysql_error());
 
$ticketsql = mysql_query("SELECT * FROM users WHERE id = '".$my_id."'") or die(mysql_error());
$ticketrow = mysql_fetch_assoc($ticketsql);


$client_port = mysql_fetch_assoc($client_port = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_port'"));
$client_mus = mysql_fetch_assoc($client_mus = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_mus'"));
$client_ip = mysql_fetch_assoc($client_ip = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_ip'"));
$client_variables = mysql_fetch_assoc($client_variables = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_variables'"));
$client_variables_night = mysql_fetch_assoc($client_variables_night = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_variables_night'"));
$client_texts = mysql_fetch_assoc($client_texts = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_texts'"));
$client_swf_path = mysql_fetch_assoc($client_swf_path = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_swf_path'"));
$client_habbo_swf = mysql_fetch_assoc($client_habbo_swf = mysql_query("SELECT * FROM cms_settings WHERE variable = 'client_habbo_swf'"));
$client_limit = mysql_fetch_assoc($client_limit = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_clientlimit'"));
if($user_rank < 1 && $online_count >= $client_limit['value']){
	
?>

<title><?php echo $shortname; ?> Hotel</title>
<div id="intermediate">
<h2><center>Límite de Usuarios</center></h2>
<div id="enter-hotel">
<div class="open enter-btn">
<a href="http://localhost" target="client" onClick="return onClientOpen(this)">Me<i></i></a><b></b>
</div>
</div>
</div>
<?php require_once('./templates/login_footer.php'); }else{ require_once('./templates/client_subheader.php'); ?>
<script type="text/javascript">
    FlashExternalInterface.loginLogEnabled = true;
    
    FlashExternalInterface.logLoginStep("web.view.start");
    
    if (top == self) {
        FlashHabboClient.cacheCheck();
    }

<?php
$pathd="http://localhost/";
?>
  var flashvars = {
    "client.allow.cross.domain" : "1",
"client.notify.cross.domain" : "0",
"connection.info.host" : "localhost",
"connection.info.port" : "90",
"site.url" : "http://localhost",
"url.prefix" : "http://localhost",
"client.reload.url" : "http://localhost/client",
"client.fatal.error.url" : "http://localhost/clientutils",
"client.connection.failed.url" : "http://localhost/clientutils",
                      "external.variables.txt" : "http://localhost/swf/gamedata/external_variables/face03d772a39a1b8ae24d1cd8da3470e586f8a9.txt",
                      "external.texts.txt" : "http://localhost/swf/gamedata/external_flash_texts/55d43ac4381c3b38a1999a4b9269c135aca3eaab.txt",
                      "productdata.load.url" : "http://localhost/swf/gamedata/productdata/Wual.txt",
                      "furnidata.load.url" : "http://localhost/swf/gamedata/furnidata_xml/Wual.xml",
"use.sso.ticket" : "1",
"sso.ticket" : "<?php echo $ticketrow['auth_ticket']; ?>",
"processlog.enabled" : "1",
"account_id" : "1",
"client.starting" : "<?php echo $shortname; ?> Hotel Carregando! ",
"flash.client.url" : "http://localhost/swf/gordon/PRODUCTION-201508190847-536139618/",
"user.hash" : "31385693ae558a03d28fc720be6b41cb1ccfec02",
"has.identity" : "1",
"flash.client.origin" : "popup"


  };
    var params = {
        "base" : "http://localhost/swf/gordon/PRODUCTION-201508190847-536139618/",
        "allowScriptAccess" : "always",
        "menu" : "false"          
  };
    
    if (!(HabbletLoader.needsFlashKbWorkaround())) {
    params["wmode"] = "opaque";
    }

    FlashExternalInterface.signoutUrl = " http://localhost/account/logout?token=<?php echo sha1($myrow['password']); ?>";
    
    var clientUrl = "http://localhost/swf/gordon/PRODUCTION-201508190847-536139618/Habbo.swf";

    swfobject.embedSWF(clientUrl, "flash-container", "100%", "100%", "10.0.0", "<?php echo $path; ?>/web-gallery/flash/expressInstall.swf", flashvars, params);
 
  window.onbeforeunload = unloading;
    function unloading() {
        var clientObject;
        if (navigator.appName.indexOf("Microsoft") != -1) {
            clientObject = window["flash-container"];
        } else {
            clientObject = document["flash-container"];
        }
        try {
            clientObject.unloading();
        } catch (e) {}
    }
</script>

<meta name="description" content="" />
<meta name="keywords" content="" />

<!--[if IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie8.css" type="text/css" />
<![endif]-->
<!--[if lt IE 8]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie.css" type="text/css" />
<![endif]-->
<!--[if lt IE 7]>
<link rel="stylesheet" href="<?php echo $path; ?>/web-gallery/static/styles/ie6.css" type="text/css" />
<script src="<?php echo $path; ?>/web-gallery/static/js/pngfix.js" type="text/javascript"></script>
<script type="text/javascript">
try { document.execCommand('BackgroundImageCache', false, true); } catch(e) {}
</script>

<style type="text/css">
body { behavior: url(/js/csshover.htc); }
</style>
<![endif]-->
</head>
<body id="client" class="flashclient">
<div id="overlay"></div>
<img src="<?php echo $path; ?>/web-gallery/v2/images/page_loader.gif" style="position:absolute; margin: -1500px;" />
<div id="overlay"></div>
<div id="client-ui" >
    <div id="flash-wrapper">
    <div id="flash-container">
        <div id="content" style="width: 400px; margin: 20px auto 0 auto; display: none">
<div class="cbb clearfix">
    <h2 class="title">Por favor, atualiza o adobe flash player para ultima versão</h2>

    <div class="box-content">
            <p>Clique Aqui: <a href="http://get.adobe.com/flashplayer/">Instala Flash player</a>. Más instrucciones para su instalación aquí: <a href="http://www.adobe.com/products/flashplayer/productinfo/instructions/">Más información</a></p>
            <p><a href="http://www.adobe.com/go/getflashplayer"><img src="<?php echo $path; ?>/web-gallery/v2/images/client/get_flash_player.gif" alt="Get Adobe Flash player" /></a></p>
    </div>
</div>

        </div>
        <script type="text/javascript">
            $('content').show();
        </script>

        <noscript>
            <div style="width: 400px; margin: 20px auto 0 auto; text-align: center">
                <p>If you are not automatically redirected, please <a href="/client">click here</a></p>
            </div>
        </noscript>
    </div>

</div>

</body retrue>

<script data-rocketsrc="<?php echo $path; ?>/habboweb/scripts.js" type="text/rocketscript"></script>
</html>

<?php } ?>