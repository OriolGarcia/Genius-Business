<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;
function generateRandomString($length = 10) {
  $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
  $charactersLength = strlen($characters);
  $randomString = '';
  for ($i = 0; $i < $length; $i++) {
      $randomString .= $characters[rand(0, $charactersLength - 1)];
  }
  return $randomString;
}
//Load Composer's autoloader
require 'vendor/autoload.php';
// STEP 1: read POST data
// Reading POSTed data directly from $_POST causes serialization issues with array data in the POST.
// Instead, read raw POST data from the input stream.
$raw_post_data = file_get_contents('php://input');

$raw_post_array = explode('&', $raw_post_data);
$myPost = array();
foreach ($raw_post_array as $keyval) {
  $keyval = explode ('=', $keyval);
  if (count($keyval) == 2)
    $myPost[$keyval[0]] = urldecode($keyval[1]);
}
// read the IPN message sent from PayPal and prepend 'cmd=_notify-validate'
$req = 'cmd=_notify-validate';
if (function_exists('get_magic_quotes_gpc')) {
  $get_magic_quotes_exists = true;
}
foreach ($myPost as $key => $value) {
  if ($get_magic_quotes_exists == true && get_magic_quotes_gpc() == 1) {
    $value = urlencode(stripslashes($value));
  } else {
    $value = urlencode($value);
  }
  $req .= "&$key=$value";
}

// Step 2: POST IPN data back to PayPal to validate
$ch = curl_init('https://ipnpb.paypal.com/cgi-bin/webscr');
curl_setopt($ch, CURLOPT_HTTP_VERSION, CURL_HTTP_VERSION_1_1);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_RETURNTRANSFER,1);
curl_setopt($ch, CURLOPT_POSTFIELDS, $req);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 1);
curl_setopt($ch, CURLOPT_SSL_VERIFYHOST, 2);
curl_setopt($ch, CURLOPT_FORBID_REUSE, 1);
curl_setopt($ch, CURLOPT_HTTPHEADER, array('Connection: Close'));
// In wamp-like environments that do not come bundled with root authority certificates,
// please download 'cacert.pem' from "https://curl.haxx.se/docs/caextract.html" and set
// the directory path of the certificate as shown below:
// curl_setopt($ch, CURLOPT_CAINFO, dirname(__FILE__) . '/cacert.pem');
if ( !($res = curl_exec($ch)) ) {
  // error_log("Got " . curl_error($ch) . " when processing IPN data");
  curl_close($ch);
  exit;
}
curl_close($ch);




// inspect IPN validation result and act accordingly
if (strcmp ($res, "VERIFIED") == 0) {
  
  // The IPN is verified, process it:
  // check whether the payment_status is Completed
  // check that txn_id has not been previously processed
  // check that receiver_email is your Primary PayPal email
  // check that payment_amount/payment_currency are correct
  // process the notification
  // assign posted variables to local variables
  $NumberSTR = $_POST['option_selection1'];
    $Number=0;
    switch ($NumberSTR) {
      case "1 PC":
          $Number =1;
          break;
      case "3 PCs":
          $Number=3;
          break;
      case "10 PCs":
          $Number=10;
          break;
    }






  $emaillicence =  $_POST['option_selection2'];
  if ($emaillicence == "") {
      $emaillicence =  $_POST['payer_email'];
    }
  $serverName = "serveetd.database.windows.net"; // update me
    $connectionOptions = array(
        "Database" => "etddatabase", // update me
        "Uid" => "tothgarsa", // update me
        "PWD" => "504C3dbb9090" // update me
    );

  
  


    $conn = sqlsrv_connect($serverName, $connectionOptions);
    var_dump($conn); 
    if( $conn ) {
     echo "Conexión establecida.<br />";
   }
   else{
    echo "Conexión no se pudo establecer.<br />";
    die( print_r( sqlsrv_errors(), true));
   }




 
   $fecha_actual = date("d-m-Y");
   //sumo 1 año
   $valida = date("d-m-Y",strtotime($fecha_actual."+ 1 year"));
       $i = 0;
       $licencias =array();
       while($i< $Number){ 
         $claves = array(
           "email" =>$emaillicence ,
           "clave" => generateRandomString(8)
         );
       $tsql= "SELECT * FROM [dbo].[Licencias] WHERE [email] = ? AND (limitdate < GETDATE() AND limitdate is not null) AND id_App = 'GNB2021'" ;
      
      
      $params=array($claves["email"]);
       $getResults= sqlsrv_query($conn, $tsql, $params);
   
         $row_count = sqlsrv_num_rows( $getResults );
       if ($row_count == 0){
         $query ="INSERT INTO [dbo].[Licencias]  ([id_App], [email], [program_key],[limitdate])     VALUES ('GNB2021', ?, ?, DATEADD(YEAR, 1, GETDATE()))";
         $params=array($claves["email"],$claves["clave"]);
         $stmt = sqlsrv_query( $conn, $query,$params);
         $i =$i +1;
       }
       while ($row = sqlsrv_fetch_array($getResults, SQLSRV_FETCH_ASSOC)){
         $query="UPDATE [dbo].[Licencias] SET program_key = ? , mac=NULL, limitdate =DATEADD(YEAR, 1, GETDATE()) Where id = ?";
        $params=array($claves["clave"],$row['id']);
         $stmt = sqlsrv_query( $conn, $query,$params);
         $i =$i +1;
         }
       sqlsrv_free_stmt($getResults);
       array_push($licencias,$claves);
      
   }
  



  $item_name = $_POST['item_name'];
  $item_number = $_POST['item_number'];
  $payment_status = $_POST['payment_status'];
  $payment_amount = $_POST['mc_gross'];
  $payment_currency = $_POST['mc_currency'];
  $txn_id = $_POST['txn_id'];
  $receiver_email = $_POST['receiver_email'];
  $payer_email = $_POST['payer_email'];
  // IPN message values depend upon the type of notification sent.
  
  $mail = new PHPMailer;
  $mail->setFrom('oriolgarciao@supremagency.com', 'Suprem Agency');
  $mail->addAddress($payer_email, 'My Friend');
  $mail->Subject  = 'Gracias por comprar Genius Business';

  $body ="<p>&nbsp;</p>";
  $body .= "<p>Hola ".$emaillicence.",</p>";
  $body .= "<p>Gracias por comprar nuestro producto de gesit&oacute;n empresarial,</p>";
  $body .= "<p>Esperamos que el producto le sea de gran utilidad.</p>";
  $body .= "<p>&nbsp;</p>";
  $body .= "<p>Estas son sus licencias:</p>";
  $body .= "<p>&nbsp;</p>.";

  foreach ($licencias as $licencia) {


    $body .= "<br><p style=\"padding-left: 90px;\">Email: {$emaillicence}</p>";
    $body .= "<p style=\"padding-left: 90px;\">Clave:{$licencia['clave']}</p>";
    $body .= "<p style=\"padding-left: 90px;\">Licencia válida hasta:{$valida}</p>";
    $body .= "<p style=\"padding-left: 90px;\">&nbsp;</p>";
  }
  $body .=  "<p>&nbsp;Si tiene algun problema no dude en contactarnos.</p>";
  $body .= "<p>&nbsp;</p>";
  $body .= "<p>Saludos cordiales.</p>";


  $cabeceras = 'MIME-Version: 1.0' . "\r\n";
  $cabeceras .= 'Content-type: text/html; charset=utf-8' . "\r\n";
  // Cabeceras adicionales

  $cabeceras .= 'From: Genius Business <oriolgarcia@supremagency.com>' . "\r\n";
  mail($payer_email, 'Gracias por comprar Genius Business', $body, $cabeceras);
  if($payer_email != $emaillicence){
        mail($emaillicence, 'Gracias por comprar Genius Business', $body, $cabeceras);
  }
}
?>