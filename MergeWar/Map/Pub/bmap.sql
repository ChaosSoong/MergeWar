-- phpMyAdmin SQL Dump
-- version phpStudy 2014
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2015 年 12 月 09 日 17:10
-- 服务器版本: 5.6.21
-- PHP 版本: 5.4.34

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `bmap`
--

-- --------------------------------------------------------

--
-- 表的结构 `circle`
--

CREATE TABLE IF NOT EXISTS `circle` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `radius` varchar(25) NOT NULL,
  `lng` varchar(18) NOT NULL,
  `lat` varchar(18) NOT NULL,
  `color` varchar(8) NOT NULL,
  `strokeWeight` tinyint(1) NOT NULL,
  `strokeOpacity` float NOT NULL,
  `fillOpacity` float NOT NULL,
  `strokeStyle` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=4 ;

-- --------------------------------------------------------

--
-- 表的结构 `line`
--

CREATE TABLE IF NOT EXISTS `line` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `parent` varchar(20) NOT NULL,
  `lng` varchar(18) NOT NULL,
  `lat` varchar(18) NOT NULL,
  `color` varchar(8) NOT NULL,
  `strokeWeight` tinyint(1) NOT NULL,
  `strokeOpacity` float NOT NULL,
  `fillOpacity` float NOT NULL,
  `strokeStyle` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=73 ;

-- --------------------------------------------------------

--
-- 表的结构 `marker`
--

CREATE TABLE IF NOT EXISTS `marker` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT '主键',
  `lat` varchar(15) NOT NULL,
  `lng` varchar(15) NOT NULL,
  `editable` tinyint(1) NOT NULL DEFAULT '1',
  `title` varchar(100) NOT NULL,
  `info` varchar(200) NOT NULL COMMENT '//简介',
  `img` varchar(150) NOT NULL COMMENT '//图片地址',
  `address` varchar(200) NOT NULL COMMENT '//地址',
  `type` tinyint(4) NOT NULL DEFAULT '2' COMMENT '//图标种类',
  `tel` varchar(20) NOT NULL COMMENT '//用户的电话',
  `link_a` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=90 ;

--
-- 转存表中的数据 `marker`
--

INSERT INTO `marker` (`id`, `lat`, `lng`, `editable`, `title`, `info`, `img`, `address`, `type`, `tel`, `link_a`) VALUES
(7, '30.554103', '104.087012', 0, '', ' BIGEMAP地图下载器', './uploads/1448508900.png', '四川省成都市天府软件园D区D7', 2, '4006-608-905', 'http://');

-- --------------------------------------------------------

--
-- 表的结构 `poly`
--

CREATE TABLE IF NOT EXISTS `poly` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `parent` varchar(20) NOT NULL,
  `lng` varchar(18) NOT NULL,
  `lat` varchar(18) NOT NULL,
  `color` varchar(8) NOT NULL,
  `strokeWeight` tinyint(1) NOT NULL,
  `strokeOpacity` float NOT NULL,
  `fillOpacity` float NOT NULL,
  `strokeStyle` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

-- --------------------------------------------------------

--
-- 表的结构 `show_info`
--

CREATE TABLE IF NOT EXISTS `show_info` (
  `id` tinyint(1) NOT NULL AUTO_INCREMENT COMMENT '序号',
  `rank` tinyint(2) NOT NULL COMMENT '地图等级',
  `core` varchar(50) NOT NULL COMMENT '中心坐标',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- 转存表中的数据 `show_info`
--

INSERT INTO `show_info` (`id`, `rank`, `core`) VALUES
(1, 6, '104.083021,30.665134');

-- --------------------------------------------------------

--
-- 表的结构 `tangle`
--

CREATE TABLE IF NOT EXISTS `tangle` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `parent` varchar(20) NOT NULL,
  `lng` varchar(18) NOT NULL,
  `lat` varchar(18) NOT NULL,
  `color` varchar(8) NOT NULL,
  `strokeWeight` tinyint(1) NOT NULL,
  `strokeOpacity` float NOT NULL,
  `fillOpacity` float NOT NULL,
  `strokeStyle` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `title_info`
--

CREATE TABLE IF NOT EXISTS `title_info` (
  `id` tinyint(1) unsigned NOT NULL AUTO_INCREMENT COMMENT '序号',
  `title` varchar(255) NOT NULL COMMENT '标题',
  `keywords` varchar(255) NOT NULL COMMENT '关键字',
  `description` varchar(255) NOT NULL COMMENT '描述',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=2 ;

--
-- 转存表中的数据 `title_info`
--

INSERT INTO `title_info` (`id`, `title`, `keywords`, `description`) VALUES
(1, 'Bigemap地图下载 成都比格图', '谷歌高清地图  Google Earth， 卫星地图 卫星图像', '谷歌卫星地图下载器高清2015 电子地图 地形图 等高线 高程（DWG矢量） 投影转换 在线标注 ');

-- --------------------------------------------------------

--
-- 表的结构 `user_list`
--

CREATE TABLE IF NOT EXISTS `user_list` (
  `userid` int(8) unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL COMMENT '用户名',
  `password` varchar(50) NOT NULL COMMENT '密码',
  `power` tinyint(1) NOT NULL COMMENT '用户权限，0:超管1:普通',
  `logintime` datetime NOT NULL COMMENT '登录时间',
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=3 ;

--
-- 转存表中的数据 `user_list`
--

INSERT INTO `user_list` (`userid`, `username`, `password`, `power`, `logintime`) VALUES
(1, 'admin', '21232f297a57a5a743894a0e4a801fc3', 0, '2015-12-09 17:10:05'),
(2, 'test', 'e10adc3949ba59abbe56e057f20f883e', 1, '2015-12-01 14:56:52');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
