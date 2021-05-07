<!DOCTYPE html>
<html>

<body>
  <h1>PHP Example used by Knet</h1>
  <p>This is a summary of the official PHP example provided by Knet.</p>
  <p>Use this example to compare the output generated by .NET Core with PHP.</p>
  <p>Note: This example needs to be run by an Apache server. You can however run it on W3 School's playground editor located here:</p>
  <a href="https://www.w3schools.com/php/phptryit.asp?filename=tryphp_intro">W3 Schools PHP Playground</a>
  <p>Just copy the entirety of this page in the playground editor and click run.</p>
  <hr/>
  <h1>Encrypt Example</h1>
  <?php
    // Resource key provided by knet
    $termResourceKey = "KK84J7YY606A4COO";
    // Knet params URL
    $knetParams = pkcs5_pad("id=000000&password=000000&action=1&langid=USA&currencycode=KWD&amt=1.000&responseURL=/cms/payment/response&errorURL==/cms/payment/error&trackid=xxxxxxx&udf1=&udf2=&udf3=&udf4=&udf5=&");
    //-----

    $encryptedParams = openssl_encrypt($knetParams, 'AES-128-CBC', $termResourceKey, OPENSSL_ZERO_PADDING, $termResourceKey);
    $encryptedParams = base64_decode($encryptedParams);
    $encryptedParams = unpack('C*', ($encryptedParams));
    $encryptedParams = byteArray2Hex($encryptedParams);
    $encryptedParams = urlencode($encryptedParams);
      
    echo $encryptedParams;

    function pkcs5_pad ($text) {
      $blocksize = 16;
      $pad = $blocksize - (strlen($text) % $blocksize);
      return $text . str_repeat(chr($pad), $pad);
    }
    
    function byteArray2Hex($byteArray) {
      $chars = array_map("chr", $byteArray);
      $bin = join($chars);
      return bin2hex($bin);
    }
  ?>

  <h1>Decrypt Example</h1>
  <?php
    // Resource key provided by knet
    $termResourceKey = "KK84J7YY606A4COO";
    // The generated encrypted param ($encryptedParams) in the first section
    $ResTranData = "eb2406719a01cfe5362416bd5a56e6d62db719da97495429d25496e58cecad0eaf3b6f044ac007468f16875b0ebb113be6756df5331cd61eb9daf8530504bf33d20b6840702717a97664f7b581756a3c93228f154e4d4e06ffd21828a0b44a6551d9fab1ced9aacfafcc8e9694382432d21148cbc9ce0b36d46342dcc69c6e850e7f2582d052432a5ec09672256d3d4a37c12ca68a4c09ce2245ed2eda902d2d65d182007671e67415093b55f8f7f39679e3749fa8d23c3d40c9656648f5a5f0";
    //-----

    $decrytedData = decrypt($ResTranData, $termResourceKey);

    echo $decrytedData;

    function decrypt($code, $key) { 
      $code = hex2ByteArray(trim($code));
      $code = byteArray2String($code);
      $iv = $key; 
      $code = base64_encode($code);
      $decrypted = openssl_decrypt($code, 'AES-128-CBC', $key, OPENSSL_ZERO_PADDING, $iv);
      return pkcs5_unpad($decrypted);
    }
      
    function hex2ByteArray($hexString) {
      $string = hex2bin($hexString);
      return unpack('C*', $string);
    }

    function byteArray2String($byteArray) {
      $chars = array_map("chr", $byteArray);
      return join($chars);
    }

    function pkcs5_unpad($text) {
      $pad = ord($text{strlen($text)-1});
      if ($pad > strlen($text)) {
        return false;	
      }
      if (strspn($text, chr($pad), strlen($text) - $pad) != $pad) {
        return false;
      }
      return substr($text, 0, -1 * $pad);
    }
  ?>

</body>

</html>
