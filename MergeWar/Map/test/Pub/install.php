<?php
$file_name="./Pub/bmap.sql"; //要导入的SQL文件名
$dbhost="localhost:3306"; //数据库主机名
$dbuser="root"; //数据库用户名
$dbpass="root"; //数据库密码
$dbname="bmap"; //数据库名
set_time_limit(0); //设置超时时间为0，表示一直执行。当php在safe mode模式下无效，此时可能会导致导入超时，此时需要分段导入
$fp = @fopen($file_name, "r") or die("不能打开SQL文件 $file_name");//打开文件
$conn=mysql_connect($dbhost, $dbuser, $dbpass) or die("不能连接数据库 $dbhost");//连接数据库
mysql_query("set names utf8",$conn); 
$sql="DROP DATABASE $dbname"; // 如果数据库存在,会删除.
mysql_query($sql);
$sql="CREATE DATABASE $dbname"; // 如果资料表存在,也会删除... 所以安全问题要考虑一下.
mysql_query($sql);
mysql_select_db($dbname) or die ("不能打开数据库 $dbname");//打开数据库 
echo "正在执行导入数据库：<BR>";
while($SQL=GetNextSQL()){
if (mysql_query($SQL)){
		echo "执行SQL：".mysql_error()."";
		echo "SQL语句为：".substr($SQL, 0,20)."...<BR>";	
	}
}
echo "导入完成<br>";
echo "<a href='./index.php' style='text-decoration:none'>如果您的浏览器没有自动跳转，请点此链接！</a>";
fclose($fp) or die("Can't close file $file_name");//关闭文件
mysql_close();
unlink('install.php');
function GetNextSQL() {
	global $fp;
	$sql="";
	while ($line = @fgets($fp, 40960)) {
		$line = trim($line);
		//以下三句在高版本php中不需要
		$line = str_replace("\\\\","\\",$line);
		$line = str_replace("\'","'",$line);
		$line = str_replace("\\r\\n",chr(13).chr(10),$line);
		// $line = stripcslashes($line);
		if (strlen($line)>1) {
			if ($line[0]=="-" && $line[1]=="-") {
					continue;
				}
		}
		$sql.=$line.chr(13).chr(10);
		if (strlen($line)>0){
			if ($line[strlen($line)-1]==";"){
				break;
			}
		}
	}
	return $sql;
}