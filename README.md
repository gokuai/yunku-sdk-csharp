/*
Title:够快云库CSharp SDK使用说明
Description:
Author: Brandon
Date: 2016/11/03
Robots: noindex,nofollow
*/

# 够快云库3.0 CSharp SDK使用说明

版本：3.0

创建：2016-11-03

## 引用

将`[yunku-csharp-sdk].dll`文件引用进项目，或者将`YunkuEntSDK`做为依赖项目。

## 初始化

要使用云库3.0的API，您需要先在 <a href="http://developer.gokuai.com/yk/tutorial#yk3" target="_blank">企业授权</a> 中获取 `client_id` 和 `client_secret`

##参数使用

以下使用到的方法中，如果是string类型的非必要参数，如果是不传，则传`null`

## 企业库管理（EntLibManager.cs）

### 构造方法

	new EntLibManager（string clientId, string clientSecret）


#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| clientId | 是 | string | 申请应用时分配的AppKey |
| clientSecret | 是 | string | 申请应用时分配的AppSecret |

---

### 创建库

	Create(string orgName, int orgCapacity, 
	string storagePointName, string orgDesc,string orgLogo) 

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgName | 是 | string | 库名称|
| orgCapacity | 否 | int | 库容量上限, 单位字节, 默认无上限|
| storagePointName | 否 | string | 库归属存储点名称, 默认使用够快存储|
| orgDesc | 否 | string | 库描述|
| orgLogo | 否 | string | 库logo |

#### 返回结果

    {
    	org_id:    
    	mount_id:
    }

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| org_id | int | 库ID |
| mount_id | int | 库空间id |

#### 数值参考

	1T="1099511627776"
	1G＝"1073741824"

---

### 修改库信息

	Set(int orgId, string orgName, string orgCapacity, string orgDesc, string orgLogo) 
	
#### 参数
 
| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| orgName | 否 | string | 库名称 |
| orgCapacity | 否 | string | 库容量限制，单位B |
| orgDesc | 否 | string | 库描述 |
| orgLogo | 否 | string | 库logo |

#### 返回结果

   正常返回 HTTP 200
   
#### 数值参考

	1T="1099511627776"
	1G＝"1073741824"
   
---

### 获取库信息

	GetInfo(int orgId)
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |

#### 返回结果

	{
      info:
      {
        org_id : 库ID
        org_name : 库名称
        org_desc : 库描述
        org_logo_url : 库图标url
        size_org_total : 库空间总大小, 单位字节, -1表示空间不限制
        size_org_use: 库已使用空间大小, 单位字节
        mount_id: 库空间id
      }
	}

---

### 获取库列表

	getLibList()

#### 参数

(无)

#### 返回结果

    {
    'list':
        [
        	{
        		org_id : 库ID
        		org_name : 库名称
        		org_logo_url : 库图标url
        		size_org_total : 库空间总大小, 单位字节, -1表示空间不限制
        		size_org_use: 库已使用空间大小, 单位字节
        		mount_id: 库空间id
        	},
        	...
        ]
    }

---

### 获取库授权

	Bind(int orgId, string title, string linkUrl)

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库ID |
| title | 否 | string | 标题(预留参数) |
| linkUrl | 否 | string | 链接(预留参数) |

#### 返回结果

	{
		org_client_id:
		org_client_secret:
	}

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| org_client_id | string | 库授权client_id |
| org_client_secret | string | 库授权client_secret |

org\_client\_secret用于调用库文件相关API签名时的密钥

---

### 取消库授权

	UnBind(string orgClientId) 

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgClientId | 是 | string | 库授权client_id |

#### 返回结果

	正常返回 HTTP 200

---

### 获取库成员列表

	GetMembers(int start, int size, int orgId)

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| start | 否 | int | 分页开始位置，默认0 |
| size | 否 | int | 分页个数，默认20 |

#### 返回结果
	
	{
		list:
		[
			{
				"member_id": 成员id,
				"out_id": 成员外部id,
				"account": 外部账号,
				"member_name": 成员显示名,
				"member_email": 成员邮箱,
				"state": 成员状态。1：已接受，2：未接受,
			},
			...
		],
		count: 成员总数
	}

---

### 查询库成员信息

	GetMember(int orgid, MemberType type, String[] ids)

#### 参数
	
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgid | 是 | int | 库id |
| type | 是 | enum | Account,OutId,MemberId |
| ids | 是 | array | 多个id数组 |

#### 返回结果
		
		{
			"id(传入时的id))":{
				"member_id": 成员id,
				"out_id": 成员外部id,
				"account": 外部账号,
				"member_name": 成员显示名,
				"member_email": 成员邮箱
			},
			...
		}

---

### 添加库成员

	AddMembers(int orgId, int roleId, int[] memberIds) 

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| roleId | 是 | int | 角色id |
| memberIds | 是 | array | 需要添加的成员id数组 |

#### 返回结果
	
	正常返回 HTTP 200

---

### 修改库成员角色

	SetMemberRole(int orgId, int roleId, int[] memberIds) 

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| roleId | 是 | int | 角色id |
| memberIds | 是 | array | 需要修改的成员id数组 |

#### 返回结果
	
	正常返回 HTTP 200

---

### 删除库成员

	DelMember(int orgId, int[] memberIds)

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| memberIds | 是 | array | 成员id数组|

#### 返回结果
	
	正常返回 HTTP 200

---

### 获取库部门列表

	GetGroups(int orgId)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |

#### 返回结果

	{
		{
			"id": 部门id
			"name": 部门名称
			"role_id": 部门角色id
		},
		...
	}

---

### 库上添加部门

	AddGroup(int orgId, int groupId, int roleId)

#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| groupId | 是 | int | 部门id|
| roleId | 否 | int | 角色id |

#### 返回结果
	
	正常返回 HTTP 200

---

### 删除库上的部门

	DelGroup(int orgId, int groupId)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| groupId | 是 | int | 部门id |

#### 返回结果
	
	正常返回 HTTP 200

---

### 修改库上部门的角色

	SetGroupRole(int orgId, int groupId, int roleId)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgId | 是 | int | 库id |
| groupId | 是 | int | 部门id |
| roleId | 否 | int | 角色id |

#### 返回结果
	
	正常返回 HTTP 200

---

### 删除库

	Destroy(String orgClientId)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| orgClientId | 否 | string | 库授权client_id|

#### 返回结果
	
	正常返回 HTTP 200

---

## 企业管理（EntManager.cs）

### 构造方法

	new EntManager（string clientId,string clientSecret，bool isEnt）

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| clientId | 是 | string | 申请应用时分配的AppKey |
| clientSecret | 是 | string | 申请应用时分配的AppSecret |
| isEnt | 是 | bool | 是否是企业帐号登录|

---

### 获取角色

	GetRoles() 

#### 参数

（无） 

#### 返回结果
	[
		{
			"id": 角色id,
			"name": 角色名称,
			},
		...
	]

---

### 获取成员列表

	GetMembers(int start, int size)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| start | 否 | int | 记录开始位置, 默认0 |
| size | 否 | int | 返回条数, 默认20 |

#### 返回结果

	{
		list:
		[
			{
				"member_id": 成员id,
				"out_id": 成员外部id,
				"account": 外部账号,
				"member_name": 成员显示名,
				"member_email": 成员邮箱,
				"state": 成员状态。1：已接受，2：未接受,
			},
			...
		],
		count: 成员总数
	}

---

### 根据成员Id查询企业成员信息

	GetMemberById(int memberId)

#### 参数 
	
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| memberId | 是 | int | 成员id |


#### 返回结果

	{
      "member_id": 成员id,
      "member_name": 成员显示名,
      "member_email": 成员邮箱,
      "out_id": 外部系统唯一id,
      "account": 外部系统登录帐号,
      "state": 成员状态。1：已接受，2：未接受
	}

---

### 根据外部系统唯一id查询企业成员信息

	GetMemberByOutId(string outId)

#### 参数 
	
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| outId | 是 | string | 外部系统唯一id|

#### 返回结果

	{
      "member_id": 成员id,
      "member_name": 成员显示名,
      "member_email": 成员邮箱,
      "out_id": 外部系统唯一id,
      "account": 外部系统登录帐号,
      "state": 成员状态。1：已接受，2：未接受
	}

---

### 根据外部系统登录帐号查询企业成员信息

	GetMemberByAccount(string account)

#### 参数 
	
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| account | 是 | String | 外部系统登录帐号 |

#### 返回结果

	{
      "member_id": 成员id,
      "member_name": 成员显示名,
      "member_email": 成员邮箱,
      "out_id": 外部系统唯一id,
      "account": 外部系统登录帐号,
      "state": 成员状态。1：已接受，2：未接受
	}

---

### 获取部门

	GetGroups() 

#### 参数 

（无）

#### 返回结果
	{
		"list":
		[
			{
				"id": 部门id,
				"name": 部门名称,
				"out_id": 外部唯一id,
				"parent_id": 上级部门id, 0为顶层
			}
		]
	}	

---

### 部门成员列表

	GetGroupMembers(int groupId, int start, int size, bool showChild) 

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| groupId | 是 | int | 部门id |
| start | 是 | int | 记录开始位置 |
| size | 是 | int | 返回条数 |
| showChild | 是 | bool | [0,1] 是否显示子部门内的成员 |

#### 返回结果
	
	
	{
		list:
		[
			{
				"member_id": 成员id,
				"out_id": 成员外部id,
				"account": 外部账号,
				"member_name": 成员显示名,
				"member_email": 成员邮箱,
				"state": 成员状态。1：已接受，2：未接受,
			},
			...
		],
		count: 成员总数
	}

---

### 根据成员id获取成员个人库外链

	GetMemberFileLink(int memberId, bool fileOnly)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| memberId | 是 | int | 成员id |
| fileOnly | 是 | bool | 是否只返回文件, 1只返回文件 |

#### 返回结果
	[
		{
		    "filename": 文件名或文件夹名,
		    "filesize": 文件大小,
    		"link": 链接地址,
    		"deadline": 到期时间戳 -1表示永久有效,
    		"password": 是否加密, 1加密, 0无
    	},
    	...
	]

---

### 添加或修改同步成员

	AddSyncMember(string oudId,string memberName,string account,string memberEmail,string memberPhone,string password)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| oudId | 是 | string | 成员在外部系统的唯一id |
| memberName | 是 | string | 显示名称 |
| account | 是 | string | 成员在外部系统的登录帐号 |
| memberEmail | 否 | string | 邮箱 |
| memberPhone | 否 | string | 联系电话 |
| password | 否 | string | 如果需要由够快验证帐号密码,密码为必须参数 |

#### 返回结果

    HTTP 200

---

### 删除同步成员

	DelSyncMember(string[] members)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| members | 是 | array | 成员在外部系统的唯一id数组|

#### 返回结果

    HTTP 200

---

### 添加或修改同步部门

	AddSyncGroup(string outId,string name,string parentOutId)

#### 参数 

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| outId | 是 | string | 部门在外部系统的唯一id |
| name | 是 | string | 显示名称 |
| parentOutId | 否 | string | 如果部门在另一个部门的下级, 需要指定上级部门唯一id, 不传表示在顶层, 修改部门时该字段无效 |

#### 返回结果

    HTTP 200

---

### 删除同步部门

	DelSyncGroup(string[]groups)
	
#### 参数 
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| groups | 是 | string | 部门在外部系统的唯一id数组|

#### 返回结果

    HTTP 200

---

### 添加同步部门的成员

	AddSyncGroupMember(string groupOutId,string[] members)
	
#### 参数 
| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| groupOutId | 否 | string | 外部部门的唯一id, 不传表示顶层 |
| members | 是 | array | 成员在外部系统的唯一id数组 |

#### 返回结果

    HTTP 200
---

### 删除同步部门的成员

	DelSyncGroupMember(string groupOutId, string[] members)
	
#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| groupOutId | 否 | string | 外部部门的唯一id, 不传表示顶层 |
| members | 是 | array | 成员在外部系统的唯一id数组 |

#### 返回结果

    HTTP 200

---

## 企业文件管理（EntFileManager.cs）

### 构造方法

	new EntFileManager(string orgClientId,string orgClientSecret);
	
#### 参数

| 参数 | 必须 | 类型 | 说明 |
| --- | --- | --- | --- |
| orgClientId | 是 | string | 库授权client_id  |
| orgClientSecret | 是 | string | 库授权client_secret  |

---

### 获取文件列表

	GetFileList( int start, string fullPath) 
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| start | 是 | int | 开始位置(每次返回100条数据) |
| fullPath | 是 | string | 文件的路径 |

#### 返回结果
	{
		count:
		list:
		[
			{
				hash:
				dir:
				fullpath:
				filename:
				filehash:
				filesize:
				create_member_name:
				create_dateline:
				last_member_name:
				last_dateline:
			},
			...
		]
	}
	
| 字段 | 类型 | 说明 |
| --- | --- | --- |
| count | int | 文件总数 |
| list | Array | 格式见下 |

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| hash | string | 文件唯一标识 |
| dir | int | 是否文件夹, 1是, 0否 |
| fullpath | string | 文件路径 |
| filename | string | 文件名称 |
| filehash | string | 文件内容hash, 如果是文件夹, 则为空 |
| filesize | long | 文件大小, 如果是文件夹, 则为0 |
| create_member_name | string | 文件创建人名称 |
| create_dateline | int | 文件创建时间戳 |
| last_member_name | string | 文件最后修改人名称 |
| last_dateline | int | 文件最后修改时间戳 |

---

### 获取更新列表

	GetUpdateList( bool isCompare, long fetchDateline)
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| mode | 否 | string | 更新模式, 可不传, 当需要返回已删除的文件时使用compare |
| fetchDateline | 是 | int | 13位时间戳, 获取该时间后的数据, 第一次获取用0 |

#### 返回结果

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| fetch_dateline | int | 当前返回数据的最大时间戳（13位精确到毫秒） |
| list | Array | 格式见下 |

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| cmd | int | 当mode=compare 时才会返回cmd字段, 0表示删除, 1表示未删除 |
| hash | string | 文件唯一标识 |
| dir | int | 是否文件夹, 1是, 0否 |
| fullpath | string | 文件路径 |
| filename | string | 文件名称 |
| filehash | string | 文件内容hash, 如果是文件夹, 则为空 |
| filesize | long | 文件大小, 如果是文件夹, 则为0 |
| create_member_name | string | 文件创建人名称 |
| create_dateline | int | 文件创建时间戳 |
| last_member_name | string | 文件最后修改人名称 |
| last_dateline | int | 文件最后修改时间戳 |

---

### 文件更新数量
	GetUpdateCounts( long beginDateline, long endDateline, bool showDelete)
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| beginDateline | 是 | long | 13位时间戳, 开始时间 |
| endDateline | 是 | long | 13位时间戳, 结束时间 |
| showDelete | 是 | boolean |是否返回删除文件 |

#### 返回结果

	{
		count: 更新数量
	}

---

### 获取文件信息

	GetFileInfo( string fullPath，NetType type) 
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| fullPath | 是 | string | 文件路径 |
| type | 是 | NetType | Default,返回公网下载地址；In，返回内网下载地址 |

#### 返回结果

	{
		hash:
		dir:
		fullpath:
		filename:
		filesize:
		create_member_name:
		create_dateline:
		last_member_name:
		last_dateline:
		uri:
		preiview：
	}

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| hash | string | 文件唯一标识 |
| dir | int | 是否文件夹 |
| fullpath | string | 文件路径 |
| filename | string | 文件名称 |
| filehash | string | 文件内容hash |
| filesize | long | 文件大小 |
| create_member_name | string | 文件创建人 |
| create_dateline | int | 文件创建时间戳(10位精确到秒)|
| last_member_name | string | 文件最后修改人 |
| last_dateline | int | 文件最后修改时间戳(10位精确到秒) |
| uri | string | 文件下载地址 |
| preiview | string | 文件预览地址 |

---

### 创建文件夹

	CreateFolder( string fullPath, string opName)
	
#### 参数 

| 参数 | 必须 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 |string| 文件夹路径 |
| opName | 是 | string | 用户名称 |

#### 返回结果

| 字段 | 类型 | 说明 |
|------|------|------|
| hash | string | 文件唯一标识 |
| fullpath | string | 文件夹的路径 |

---

### 通过文件流上传（50M以内文件）

	CreateFile( string fullPath,string opName, FileInputStream stream) 
	
#### 参数

| 参数 | 必须 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 | string | 文件路径 |
| opName | 是 | string | 用户名称 |
| stream | 是 | stream | 文件流 |


#### 返回结果

| 字段 | 类型 | 说明 |
|------|------|------|
| hash | string | 文件唯一标识 |
| fullpath | string | 文件路径 |
| filehash | string | 文件内容hash |
| filesize | long | 文件大小 |

---

### 通过本地路径上传（50M以内文件）

	CreateFile( string fullPath, string opName, string localPath)
	
#### 参数

| 参数 | 必须 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 | string | 文件路径 |
| opName | 时 | string | 用户名称 |
| localPath | 是 | string | 文件流 |

#### 返回结果

| 字段 | 类型 | 说明 |
|------|------|------|
| hash | string | 文件唯一标识 |
| fullpath | string | 文件路径 |
| filehash | string | 文件内容hash |
| filesize | long | 文件大小 |

---

### 通过本地路径上传（50M以内文件）

	CreateFile( string fullPath, string opName, string localPath)
	
#### 参数 
| 参数 | 必须 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 | string | 文件路径 |
| opName | 否 | string | 用户名称 |
| localPath | 是 | string | 文件流 |
| fileName | 是 | string | 文件名 |

#### 返回结果

| 字段 | 类型 | 说明 |
|------|------|------|
| hash | string | 文件唯一标识 |
| fullpath | string | 文件路径 |
| filehash | string | 文件内容hash |
| filesize | long | 文件大小 |

---

### 文件分块上传

	UploadByBlock( String fullPath, String opName,
	 int opId, String localFilePath,boolean overWrite,CompletedHanlder completeHanlder,ProgressChangedHandler progressHandler)
	
#### 参数 
| 参数 | 必须 | 类型 | 说明 |	
|------|------|------|------|
| fullpath | 是 | string | 文件路径 |
| opName | 否 | string |  创建人名称, 如果指定了op_id, 就不需要op_name， |
| opId | 否 | int | 创建人id, 个人库默认是库拥有人id, 如果创建人不是云库用户, 可以用op_name代替,|
| localFilePath | 是 | string | 文件本地路径 |
| overWrite | 是 | boolean | 是否覆盖同名文件，true为覆盖 |
| completeHanlder | 否 | CompletedHanlder | 上传完成回调 |
| progressHandler | 否 | ProgressChangedHandler | 上传进度回调 |

---

### 移动文件

	Move( string fullPath, string destFullPath, string opName)
	
#### 参数 

| 参数 | 必需 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 | string | 要移动文件的路径 |
| destFullPath | 是 | string | 移动后的路径 |
| opName | 是 | string | 用户名称 |

#### 返回结果
	正常返回 HTTP 200
	
---

### 获取文件链接

	Link( String fullPath, int deadline, AuthType authType, String password)
	
#### 参数 
| 参数 | 必需 | 类型 | 说明 |
|------|------|------|------|
| fullPath | 是 | string | 文件路径 |
| deadline | 否 | int | 10位链接失效的时间戳 ，永久传－1 |
| authtype | 是 | enum | 文件访问权限DEFAULT默认为预览，PREVIEW：预览，DOWNLOAD：下载、预览，UPLOAD：上传、下载、预览｜
| password | 否 | string | 密码，不过不设置密码，传null |

#### 返回结果

### 发送消息

	Sendmsg( string title, string text, string image, string linkUrl, string opName) 
	
#### 参数

| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| title | 是 | string | 消息标题 |
| text | 是 | string | 消息正文 |
| image | 否 | string | 图片url |
| linkUrl | 否 | string | 链接 |
| opName | 是 | string | 用户名称 |

#### 返回结果
	正常返回 HTTP 200 
	
---

### 获取当前库所有外链

	Links( bool fileOnly)
	
#### 参数 
| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| fileOnly | 是 | bool | 是否只返回文件, 1只返回文件 |
#### 返回结果
	[
		{
		    "filename": 文件名或文件夹名,
		    "filesize": 文件大小,
    		"link": 文件外链地址,
    		"deadline": 到期时间戳 -1表示永久有效,
    		"password": 是否加密, 1加密, 0无
    	},
    	...
	]

---

### 通过链接上传文件
	
	CreateFileByUrl(string fullpath,int opId,string opName,bool overwrite,string url)

#### 参数 
| 名称 | 必需 | 类型 | 说明 |
| --- | --- | --- | --- |
| fullpath | 是 | string | 文件路径 |
| opId | 否 | int | 创建人id, 个人库默认是库拥有人id, 如果创建人不是云库用户, 可以用op_name代替|
| opName | 否 | string | 创建人名称, 如果指定了opId, 就不需要opName|
| overwrite | 是 | bool | 是否覆盖同名文件, true覆盖(默认) false不覆盖,文件名后增加数字标识|
| url | 是 | string | 需要服务端获取的文件url|

#### 返回结果
	正常返回 HTTP 200 

---

### WEB直接上传文件
	GetUploadServers()

#### 参数 

(无)

#### 返回结果
	{
       "upload":
       [
          上传服务器地址 如:http://upload.domain.com,
         ...
       ]
	}

---


