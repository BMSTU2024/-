<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'myDB';
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database.';charset=UTF8', $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has occurred.</h1><pre>', $e->getMessage()
            ,'</pre>';
}
$sth = $dbh->prepare(
	'insert into myDB.message( sender,text,data) values (:pr_pl,:pr_tx,now())'

);
mysql_set_charset("UTF8");
$sth->bindParam(':pr_pl',$_POST['php_pl'],PDO::PARAM_INT);
$sth->bindParam(':pr_tx',$_POST['php_tx'],PDO::PARAM_STR);
echo $_POST['php_tx'];
$dbh->query('SET NAMES UTF8');
$sth->execute();

?>