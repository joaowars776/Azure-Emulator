<?php

require_once('./data_classes/server-data.php_data_classes-core.php.php');
require_once('./data_classes/server-data.php_data_classes-session.php.php');

$pagename = "Mídias Sociais";
$pageid = "11b";
$body_id = "newcredits";

require_once('./templates/community_subheader.php');
require_once('./templates/community_header.php'); 

?> 

<div id="container">
<div id="content" style="position: relative" class="clearfix">
<div id="column1" class="column">
<div class="habblet-container ">
<div class="cbb clearfix orange ">
<h2 class="title">Fã Sites</h2>
<div id="community-content">
<h3>   Você tem um Fã site? Ele pode estar nesta lista!</h3>
<strong>   Official Fansites</strong><br/>
   <a>Não temos fã sites ainda!</a><br/><br/>								</div>
				</div>
			</div>
		<script type="text/javascript">if (!$(document.body).hasClassName('process-template')) { Rounder.init(); }</script>
		</div>

		<div id="column2" class="column"></div>
		<script type="text/javascript">
			HabboView.run();
		</script>
<?php require_once('./templates/community_footer.php'); ?>
		</div>
	</div>
</div>
</body>