<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }

if(empty($body_id)){
$body_id = "home";
}

?>
<body id="<?php echo $body_id; ?>" class=""<?php if($pageid == "4"){ ?> onload="NaviFenster()"<?php } ?>>
<div id="overlay"></div>

<div id="header-container">
	<div id="header" class="clearfix">
		<h1><a href="index"></a></h1>
       <div id="subnavi">
			<div id="subnavi-user">
                            <?php if($logged_in){ ?>
				</ul>

                            <?php } elseif(!$logged_in){ ?>
                                <div class="clearfix">&nbsp;</div>
        <p>
                <a href="/client" id="enter-hotel-open-medium-link" target="client" onclick="HabboClient.openOrFocus(this); return false;">Entrar no <?php echo $shortname; ?> Hotel</a>
        </p>

                            <?php } ?>

			</div>
        <?php if(!$logged_in){ ?>

        </div>
        <?php } else { ?>
            <div id="subnavi-search">
                <div id="subnavi-search-upper">
                <ul id="subnavi-search-links">
					<li><a href="/sair" class="userlink">Sair</a></li>
				</ul>
                </div>


<div id="to-hotel">

<?php
$a = mysql_query("SELECT id,hotel,data FROM client_status WHERE hotel='0'");
while($b = mysql_fetch_array($a)){
?>
<div id="hotel-closed-medium">Hotel está Offline</div>
<?php } ?>
<?php
$a = mysql_query("SELECT * FROM client_status WHERE hotel='1'");
while($b = mysql_fetch_array($a)){
?>
<a href="/client" id="enter-hotel-open-medium-link" target="68fab055439ecc0ae483343b85d39f9f87f4770f" onclick="HabboClient.openOrFocus(this); return false;">Voltar ao Hotel</a>
<?php } ?>

</div>

	   
</div>


        </div>
        <?php } ?>
        <ul id="navi">
        <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile" && $logged_in == true){ ?>
        <li class="selected"><strong>
	<?php echo $name; ?> (<img src="<?php echo $path; ?>/web-gallery/v2/images/icon_habbo_small.png">)
		</strong><span></span></li>
        <?php } elseif($logged_in == true){ ?>
        <li class=" "><a href="<?php echo $path; ?>/me">
	<?php echo $name; ?> (<img src="<?php echo $path; ?>/web-gallery/v2/images/icon_habbo_small.png">)
		</a><span></span></li>
        <?php } elseif($logged_in !== true){ ?>
        <li id="tab-register-now"><a href="<?php echo $path; ?>/register" target="_self">Registe-se agora!</a><span></span></li>
        <?php } ?>

        <?php if($pageid >= 10 && $pageid <= 20 || $pageid == "profile"){ ?>
        <li class="selected"><strong>Comunidade</strong><span></span></li>
        <?php } else { ?>
        <li class=" "><a href="<?php echo $path; ?>/comunidade">Comunidade</a><span></span></li>
        <?php } ?>

        <?php if($pageid >= 5 && $pageid <= 9){ ?>
        <li class="selected"><strong>Estatísticas</strong><span></span></li>
        <?php } else { ?>
        <li class=""><a href="<?php echo $path; ?>/status">Estatísticas</a><span></span></li>
        <?php } ?>
		
        <?php if($pageid >= 1000 && $pageid <= 1000){ ?>
        <li class="selected"><strong>Famosos</strong><span></span></li>
        <?php } else { ?>
        <li class=""><a href="<?php echo $path; ?>/hall/inicio">Famosos</a><span></span></li>
        <?php } ?>

        <?php if($user_rank > 6 && $logged_in == true){ ?>
        <li id="tab-register-now"><a href="<?php echo $adminpath; ?>" target="_blank"><b>Painel</b></a><span></span></li>
        <?php } ?>
</ul>
	<div id="habbos-online">
	
	<?php
$a = mysql_query("SELECT * FROM client_status WHERE hotel='0'");
while($b = mysql_fetch_array($a)){
?>
	
    <?php } ?>
	<?php
$a = mysql_query("SELECT * FROM client_status WHERE hotel='1'");
while($b = mysql_fetch_array($a)){
?>
	<div class="rounded"><span><?php echo $online_count; ?> <?php echo $shortname; ?>es no Hotel</span></div>
	<?php } ?>
	
	</div>
	</div>
</div>

<div id="content-container">

<div id="navi2-container" class="pngbg">
    <div id="navi2" class="pngbg clearfix">

	<ul>
        <?php if($pageid > 0 && $pageid < 5 || $pageid == "myprofile"){ ?>
                <?php if($pageid == "1"){ ?>
                <li class="selected">
    		Home 

                <?php } else { ?>
                <li class="">
    		<a href="<?php echo $path; ?>/me">Home</a>
                <?php } ?>
        		</li>

                <?php if($pageid == "2"){ ?>
                <li class="selected">
    				Preferências
                <?php } elseif($logged_in){ ?>
                <li class="">
	    			<a href="<?php echo $path; ?>/profile">Preferências</a>
                <?php } ?>
        		</li>
                
                <?php } else if($pageid >= 10 && $pageid <= 20 || $pageid == "profile"){ ?>
                <?php if($pageid == "10"){ ?>
                <li class="selected">
    				Comunidade
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/comunidade">Comunidade</a>
                <?php } ?>

                <?php if($pageid == "11"){ ?>
                <li class="selected">
    				Notícias
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/comunidade/articles">Notícias</a>
                <?php } ?>

                <?php if($pageid == "13"){ ?>
                <li class="selected">
				Equipe
		        <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/comunidade/staff">Equipe</a>
                <?php } ?>
				
				<?php if($pageid == "11b"){ ?>
                <li class="selected">
    				Fã sites
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/comunidade/fansites">Fã sites</a>
	    			
                <?php } ?>
                
                <?php } else if($pageid >= 1000  && $pageid <= 1000){ ?>
                <?php if($pageid == "1000"){ ?>
                <li class="selected ">
    				Hall da Fama
                <?php } else { ?>
                <li class=" ">

                <?php } ?>
                <?php if($pageid == "6b"){ ?>
                <li class="selected">
    				
                <?php } else { ?>
                <li class=" ">
	    			
                <?php } ?>
				<?php } else if($pageid >= 5  && $pageid <= 9){ ?>
                <?php if($pageid == "5"){ ?>
                <li class="selected ">
    				Servidor
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/bots">Bots <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php } else if($pageid >= 40  && $pageid <= 47){ ?>
                <?php if($pageid == "40"){ ?>
                <li class="selected ">
    				Principal <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda">Principal <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "41"){ ?>
                <li class="selected ">
    				Emblemas <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/placas">Emblemas <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "42"){ ?>
                <li class="selected ">
    				Raros <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/rares">Raros <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "43"){ ?>
                <li class="selected ">
    				Respeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/respetos">Respeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "44"){ ?>
                <li class="selected ">
    				Cavalos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/caballos">Cavalos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "45"){ ?>
                <li class="selected ">
    				Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/efectos">Efeitos <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
                <?php if($pageid == "47"){ ?>
                <li class="selected ">
    				Vip <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png">
                <?php } else { ?>
                <li class=" ">
	    			<a href="<?php echo $path; ?>/tienda/vip">Vip <img src="<?php echo $path; ?>/web-gallery/v2/images/star.png"></a>
                <?php } ?>
				


<?php } ?>
    	    	</li>
</ul>


	</div>
</div>