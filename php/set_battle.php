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
//запрос с добавлением сражения в бд. Сначала делается запрос с добавлением сражения и получением его индекса, затем изменение записи сражения и добавления записей в историю сражений победителя и проигравшего(эти запросы выполняются в разное время)
$par_bl=(int)$_POST['php_bl'];
if($par_bl==1){
		$sth = $dbh->prepare(
	'update myDB.battle set myDB.battle.end=NOW() where (myDB.battle.id=:pr_id)'

	);
	$sth->bindParam(':pr_id',$_POST['php_id'],PDO::PARAM_INT);
$sth->execute();

$sth = $dbh->prepare('insert into myDB.history_battles(history_battles.id_battle,history_battles.name_player,history_battles.status) values (:pr_id,:pr_player,:pr_status)');
$sth->bindParam(':pr_id',$_POST['php_id'],PDO::PARAM_INT);
$sth->bindParam(':pr_player',$_POST['php_player_win'],PDO::PARAM_STR);
$win='win';
$sth->bindParam(':pr_status',$win,PDO::PARAM_STR);
$dbh->query('SET NAMES UTF8');
$sth->execute();

$sth = $dbh->prepare('insert into myDB.history_battles(history_battles.id_battle,history_battles.name_player,history_battles.status) values (:pr_id,:pr_player,:pr_status)');
$sth->bindParam(':pr_id',$_POST['php_id'],PDO::PARAM_INT);
$sth->bindParam(':pr_player',$_POST['php_player_lose'],PDO::PARAM_STR);
$lose='lose';
$sth->bindParam(':pr_status',$lose,PDO::PARAM_STR);
$dbh->query('SET NAMES UTF8');
$sth->execute();
}
else{
	$sth = $dbh->prepare(
	'insert into myDB.battle(myDB.battle.start) values (NOW())'

	);
	

	$sth->execute();

$sth = $dbh->prepare('SELECT * FROM myDB.battle order by myDB.battle.id desc limit 1');


$sth->execute();
$sth->setFetchMode(PDO::FETCH_ASSOC);
$result = $sth->fetchAll(); 
if (count($result) > 0) 
{
	foreach($result as $r) 
	{
		echo $r['id'],"_";

	}

}

echo 'good';
}
	//$sth->bindParam(':pr_id',$_GET['php_id'],PDO::PARAM_INT);


?>