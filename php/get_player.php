<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'myDB';
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has occurred.</h1><pre>', $e->getMessage()
            ,'</pre>';
}
//запрос с получением игрока с определённым логином, паролем и статусом 
$sth = $dbh->prepare('SELECT * FROM myDB.player WHERE (myDB.player.login = :pr_login and myDB.player.password=:pr_password and myDB.player.status=:pr_status)');

$sth->bindParam(':pr_login',$_GET['php_login'],PDO::PARAM_STR);
$sth->bindParam(':pr_status',$_GET['php_status'],PDO::PARAM_STR);
$sth->bindParam(':pr_password',$_GET['php_password'],PDO::PARAM_INT);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['login'],"_";
		echo $r['password'], "_";
	}
}
?>