<?php
Header("Content-type: image/png");
 
if(isset($_GET['img']))
        $linkYT = $_GET['img'];
       
echo file_get_contents('[url]http://i1.ytimg.com/vi/'.[/url] $linkYT .'/default.jpg');
?>