<?php
/*=========================================================+
|| # Xdr 2013.
|| # Este es un Software de código libre, libre edición.
|| # Build: 2500 (R2.5)
|+=========================================================+
|| # Based in Jaym/Kreechin.
|+=========================================================+
*/

$Base = false;
$Symbols = false;
$Dir = 'BN';

$BaseCode = '';
$SymbolsCode = array();
$array11 = array('<', '>', '"', '\'', '\\');
$array22 = array('&lt;', '&gt;', '&quot;', '&#39;', '&#92');
$_GET = str_replace($array11,$array22, $_GET);

if(isset($_SERVER['HTTP_REFERER']) && (strstr($_SERVER['HTTP_REFERER'], '/groups/') || strstr($_SERVER['HTTP_REFERER'], '/myhabbo/') || strstr($_SERVER['HTTP_REFERER'], '/community'))):
	$Dir = 'BO';
endif;

//Methods
function GetParts($BasePartCode, $isSymbol = false) {
	$BadgePartCode = array();

	if(strlen($BasePartCode) == 4 || strlen($BasePartCode) == 5):
		if(!$isSymbol && strlen($BasePartCode) == 5):
			$_Codes = str_split($BasePartCode, 3);
			$__Codes = str_split($_Codes[1], 2);

			$Codes[0] = $_Codes[0];
			$Codes[1] = $__Codes[0];
		else:
			$Codes = str_split($BasePartCode, 2);
		endif;
	elseif(strlen($BasePartCode) == 6):
		$_Codes = str_split($BasePartCode, 3);
		$__Codes = str_split($_Codes[1], 2);

		$Codes[0] = $_Codes[0];
		$Codes[1] = $__Codes[0];
		$Codes[2] = $__Codes[1];
	endif;

	$BadgePartCode[0] = $Codes[0];

	$Colors = array('01' => '0xff0xd60x01',
		'02' => '0xee0x760x00',
		'03' => '0x840xde0x00',
		'04' => '0x580x9a0x00',
		'05' => '0x500xc10xfb',
		'06' => '0x000x6f0xcf',
		'07' => '0xff0x980xe3',
		'08' => '0xf30x340xbf',
		'09' => '0xff0x2d0x2d',
		'10' => '0xaf0x0a0x0a',
		'11' => '0xff0xff0xff',
		'12' => '0xc00xc00xc0',
		'13' => '0x370x370x37',
		'14' => '0xfb0xe70xac',
		'15' => '0x970x760x41',
		'16' => '0xc20xea0xff',
		'17' => '0xff0xf10x65',
		'18' => '0xaa0xff0x7d',
		'19' => '0x870xe60xc8',
		'20' => '0x980x440xe7',
		'21' => '0xde0xa90xff',
		'22' => '0xff0xb50x79',
		'23' => '0xc30xaa0x6e',
		'24' => '0x7a0x7a0x7a'
	);

	$BadgePartCode[1] = (isset($Colors[$Codes[1]])) ? $Colors[$Codes[1]] : '';
	if(isset($Codes[2]))	$BadgePartCode[2] = $Codes[2];

	return $BadgePartCode;
}

function CheckStr($Str, $Space = true) {
	return ($Space) ? preg_match('/^[a-z0-9-\s]*\$/i', $Str) : preg_match('/^[a-z0-9-]*\$/i', $Str);
}

function Colorize($IMG, $RGB) {
  	imageTrueColorToPalette($IMG, true, 256);
  	$numColors = imageColorsTotal($IMG);

 	for ($x = 0; $x < $numColors; $x++) {
		list($r, $g, $b) = array_values(imageColorsForIndex($IMG, $x));
		$grayscale = ($r + $g + $b) / 3 / 0xff;
		imageColorSet($IMG, $x, $grayscale * $RGB[0], $grayscale * $RGB[1], $grayscale * $RGB[2]);
	}
}

// Security
if(!isset($_GET['badge']) || CheckStr($_GET['badge']))
	exit;
$testete= "'.gif', 'X'";
$BadgeCode = str_replace($testete, '', $_GET['badge']);
// Cache
if(file_exists($Dir . '/cache/' . $BadgeCode . '.gif')):
	header('Content-type: image/gif');
	imagegif(imagecreatefromgif($Dir . '/cache/' . $BadgeCode . '.gif'));
	exit;
endif;
// Generator
if(strstr($BadgeCode, 'b'))
	$Base = true;

if(strstr($BadgeCode, 's'))
	$Symbols = true;

if($Symbols === true):
	$Parts = explode('s', $BadgeCode);
	$BaseCode = str_replace('b', '', $Parts[0]);

	if(strlen($BaseCode) < 4 && strlen($BaseCode) > 5)
		exit;

	if(strlen($Parts[1]) >= 4 && strlen($Parts[1]) <= 6)
		$SymbolsCode[0] = $Parts[1];
	if(isset($Parts[2]) && strlen($Parts[2]) >= 4 && strlen($Parts[2]) <= 6)
		$SymbolsCode[1] = $Parts[2];
	if(isset($Parts[3]) && strlen($Parts[3]) >= 4 && strlen($Parts[3]) <= 6)
		$SymbolsCode[2] = $Parts[3];
	if(isset($Parts[4]) && strlen($Parts[4]) >= 4 && strlen($Parts[4]) <= 6):
		$SymbolsCode[3] = $Parts[4];
	endif;
else:
	$BaseCode = str_replace('b', '', $BadgeCode);
	if(strlen($BaseCode) < 3 || strlen($BaseCode) > 5)
		exit;
endif;
$Gif_Base = imagecreatefromgif($Dir . '/base/base.gif');
$Gif_Symbol = imagecreatefromgif($Dir . '/templates/none.gif');
//Base
if($Base):
	$BasePartArray = GetParts($BaseCode);
	$ColourHex = str_split($BasePartArray[1], 4);

	$BaseIMG = imagecreatefromgif($Dir . '/base/' .  $BasePartArray[0] . '.gif');
	$p = (imageSX($Gif_Base) / 2) - (imageSX($BaseIMG) / 2);
	$pp = (imageSY($Gif_Base) / 2) - (imageSY($BaseIMG) / 2);

	$Image = getimagesize ($Dir . '/base/' .  $BasePartArray[0] . '.gif');

	if(!empty($BasePartArray[1]))
		Colorize($BaseIMG, $ColourHex);

	if(file_exists($Dir . '/base/' .  $BasePartArray[0] . '_' .  $BasePartArray[0] . '.gif')):
		$BaseIMG2 = imagecreatefromgif($Dir . '/base/' .  $BasePartArray[0] . '_' .  $BasePartArray[0] . '.gif');
		imagecopymerge($BaseIMG, $BaseIMG2, 0, 0, 0, 0, $Image[0], $Image[1], 100);

		imagecopy($Gif_Base, $BaseIMG, $p, $pp, 0, 0, $Image[0], $Image[1]);
	else:
		imagecopy($Gif_Base, $BaseIMG, $p, $pp, 0, 0, $Image[0], $Image[1]);
	endif;
endif;
//Symbols
if($Symbols):
	foreach($SymbolsCode as $SymbolCode) {
		$SymbolPartArray = GetParts($SymbolCode, true);
		if(!file_exists($Dir . '/templates/' . $SymbolPartArray[0] . '.gif'))
			continue;

		$SymbolIMG = imagecreatefromgif($Dir . '/templates/' . $SymbolPartArray[0] . '.gif');
		$Image = getimagesize ($Dir . '/templates/' . $SymbolPartArray[0] . '.gif');
		$h = $Image[0];
		$w = $Image[1];

		if(!isset($SymbolPartArray[2]) || $SymbolPartArray[2] < 0 || $SymbolPartArray[2] > 8)
			$Pos = 0;
		else
			$Pos = $SymbolPartArray[2];

		$p = '0';
		$pp = '0';
		$sSymbolIMG_Width = imageSX($Gif_Base);
		$SymbolIMG_Width = imageSX($SymbolIMG);
		$sSymbolIMG_Height = imageSY($Gif_Base);
		$SymbolIMG_Height = imageSY($SymbolIMG);

		if($Pos == 0):
			$p = '0';
			$pp = '0';
		elseif($Pos == 1):
			$p = (($sSymbolIMG_Width - $SymbolIMG_Width) / 2);
			$pp = 0;
		elseif ($Pos == 2):
			$p = $sSymbolIMG_Width - $SymbolIMG_Width;
			$pp = 0;
		elseif($Pos == 3):
			$p = 0;
			$pp = ($sSymbolIMG_Height / 2) - ($SymbolIMG_Height / 2);
		elseif($Pos == 4):
			$p = ($sSymbolIMG_Width / 2) - ($SymbolIMG_Width / 2);
			$pp = ($sSymbolIMG_Height / 2) - ($SymbolIMG_Height / 2);
		elseif($Pos == 5):
			$p = $sSymbolIMG_Width - $SymbolIMG_Width;
			$pp = ($sSymbolIMG_Height / 2) - ($SymbolIMG_Height / 2);
		elseif($Pos == 6):
			$p = 0;
			$pp = $sSymbolIMG_Height - $SymbolIMG_Height;
		elseif($Pos == 7):
			$p = (($sSymbolIMG_Width - $SymbolIMG_Width) / 2);
			$pp = $sSymbolIMG_Height - $SymbolIMG_Height;
		elseif($Pos == 8):
			$p = $sSymbolIMG_Width - $SymbolIMG_Width;
			$pp = $sSymbolIMG_Height - $SymbolIMG_Height;
		endif;

		if(file_exists($Dir . '/templates/' . $SymbolPartArray[0] . '_' . $SymbolPartArray[0] . '.gif')):
			$SymbolIMG2 = imagecreatefromgif($Dir . '/templates/' . $SymbolPartArray[0] . '_' . $SymbolPartArray[0] . '.gif');

			if(!empty($SymbolPartArray[1]) && $SymbolPartArray[0] != 209 && $SymbolPartArray[0] != 210 && $SymbolPartArray[0] != 211 && $SymbolPartArray[0] != 212)
				Colorize($SymbolIMG, str_split($SymbolPartArray[1], 4));

			imagecopymerge($SymbolIMG, $SymbolIMG2, 0, 0, 0, 0, $Image[0], $Image[1], 100);
			imagecopy($Gif_Base, $SymbolIMG, $p, $pp, 0, 0, $Image[0], $Image[1]);
		else:
			if(!empty($SymbolPartArray[1]))
				Colorize($SymbolIMG, str_split($SymbolPartArray[1], 4));

			imagecopy($Gif_Base, $SymbolIMG, $p, $pp, 0, 0, $Image[0], $Image[1]);
		endif;
	}
endif;
header('Content-type: image/gif');
//imagegif($Gif_Base, $Dir . '/cache/' . $BadgeCode . '.gif');
imagegif($Gif_Base);
?>