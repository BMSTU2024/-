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
 
$sth = $dbh->query('SELECT * FROM myDB.player');
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