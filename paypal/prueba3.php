<?php


use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;
require 'vendor/autoload.php';
function generateRandomString($length = 10) {
  $characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
  $charactersLength = strlen($characters);
  $randomString = '';
  for ($i = 0; $i < $length; $i++) {
      $randomString .= $characters[rand(0, $charactersLength - 1)];
  }
  return $randomString;
}

  
  // The IPN is verified, process it:
  // check whether the payment_status is Completed
  // check that txn_id has not been previously processed
  // check that receiver_email is your Primary PayPal email
  // check that payment_amount/payment_currency are correct
  // process the notification
  // assign posted variables to local variables
  $NumberSTR = "10 PCs";
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


  $emaillicence =  "ogs1017@hotmail.com";
  if ($emaillicence == "") {
      $emaillicence =  "ogs1017@hotmail.com";
    }
  $serverName = "serveetd.database.windows.net"; // update me
    $connectionOptions = array(
        "Database" => "etddatabase", // update me
        "Uid" => "tothgarsa", // update me
        "PWD" => "504C3dbb9090" // update me
    );

   $conn = sqlsrv_connect($serverName, $connectionOptions);
   var_dump($conn); 


?>