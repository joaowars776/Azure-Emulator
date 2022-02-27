<?php if (!defined("IN_HOLOCMS")) { header("Location: $path"); exit; }
if(!isset($body_id)){ $body_id = "landing"; }

$get_cc = mysql_query("SELECT * FROM cms_settings WHERE variable = 'cms_flashclient' AND example = '1'");
if(mysql_num_rows($get_cc) > 0){
$stats = "online";
}else{
$stats = "offline";
}

?>

<body id="<?php echo $body_id; ?>" class="process-template">

<div id="overlay"></div>

<div id="container">
	<div class="cbb process-template-box clearfix">
		<div id="content">
			<div id="header" class="clearfix">
				<h1><a href="<?php echo $path; ?>"></a></h1>
				<ul class="stats">
					    <li class="stats-online"><span class="stats-fig"><?php echo $online_count; ?></span> <?php echo $shortname; ?>s Online</li> 
					    <li class="stats-visited"><img src='../web-gallery/v2/images/<?php echo $stats; ?>.gif' alt='Server Status' border='0'></li>
				</ul>
			</div>