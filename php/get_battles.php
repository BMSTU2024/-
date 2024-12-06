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
//запрос с отображением сражений, где указывается, противник, длительность сражения и результат(для основного игрока- тот у кого будет отображаться).Сортирует по дате
$sth=null;
/*
	$sth = $dbh->prepare(
	'SELECT (bt.end - bt.start) as time_long, h_b.name_player,
h_b.STATUS
FROM myDB.history_battles AS h_b
INNER JOIN myDB.battle AS bt ON ( id = id_battle )
WHERE (
h_b.name_player = :pr_name
)
ORDER BY bt.start'	);

*/
$sth=$dbh->prepare('SELECT (bt.end - bt.start) as time_long, h_b.name_player,
h_b.status
FROM myDB.history_battles AS h_b,myDB.battle AS bt,myDB.history_battles AS h_b2
WHERE (
h_b.name_player != :pr_name and 
bt.id=h_b.id_battle and h_b2.id_battle=bt.id  and h_b2.name_player=:pr_name1)
ORDER BY bt.start desc');
$name=$_GET['php_player'];
//echo $name;
$sth->bindParam(':pr_name',$name,PDO::PARAM_STR);
$sth->bindParam(':pr_name1',$name,PDO::PARAM_STR);
$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['name_player'],"_";
		echo $r['time_long'],"_";
		echo $r['status'],"_";

	}
}
?>