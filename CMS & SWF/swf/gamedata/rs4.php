<?php

/*
 *****************
 * @author capos *
 *****************
*/

if(!isset($_GET['token']) || strlen(trim($_GET['token'])) < 10)
	exit(header('Location: ../error/500?tstamp=' . date('YmdHis')));

$w = 100;
$h = 114;

$pixels = file_get_contents('banner.txt'); // the banner pixels
$token = trim($_GET['token']);
$prime = '114670925920269957593299136150366957983142588366300079186349531';
$generator = '1589935137502239924254699078669119674538324391752663931735947';

$insert = chr(strlen($prime)) . $prime . chr(strlen($generator)) . $generator;
$Length = strlen($token);
$Length2 = strlen($insert);
$p = 0;
$bitsnum = '';
$insertpos = 0;

for($i = 0; $i < $Length2; $i++):
	$bits = base_convert(ord($insert[$i]) ^ ord($token[$p]), 10, 2);
	$need = 8 - strlen($bits);
	for($o=0;$o<$need;$o++)	$bits = '0' . $bits;
	$bitsnum .= $bits;
	if (++$p == $Length) $p = 0;
endfor;

$Length = strlen($bitsnum);

for ($y = 39; $y < 69; $y++):
	$a = 0;
	for ($r = 4; $r < 84; $r++):
		$pos = (($y + $a) * $w + $r) * 4;
		$b = 1;
		while ($b < 4):
			if($insertpos < $Length):
				$binaryData = base_convert(ord($pixels[$pos + $b]), 10, 2);
				$need = 8 - strlen($binaryData);
				for($o = 0; $o < $need; $o++) $binaryData = '0' . $binaryData;
				$binaryData[7] = $bitsnum[$insertpos];
				$pixels[$pos + $b] = chr(base_convert($binaryData, 2, 10));
				$insertpos++;$b++;
				continue;
			endif;
			break 3;
		endwhile;
		if ($r % 2 == 0) $a++;
	endfor;
endfor;

$i = imagecreatetruecolor($w, $h);
imagealphablending($i, false);
imagesavealpha($i, true);
$x = 0;
$y = 0;

$c = unpack('N*', $pixels);
foreach($c as $C):
	imagesetpixel($i, $x, $y, (0x7f - ($C >> 25) << 24) | ($C & 0xffffff));

	if(++$x== $w):
		$x = 0;
		$y++;
	endif;
endforeach;

header('Content-Type: image/png');
imagepng($i);
?>