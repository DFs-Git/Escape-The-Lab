README.txt - 化学反应方程式数据库格式说明

1. 文件概述
本JSON文件是一个化学反应方程式的数据库，包含多种无机化学反应，主要用于化学教学和参考。文件采用UTF-8编码。

2. 数据结构
整个文件是一个JSON数组，每个元素代表一个化学反应，包含以下字段：

- "反应物"：数组，包含反应物的化学式及其系数
- "反应条件"：字符串，描述反应发生的条件
- "生成物"：数组，包含生成物的化学式及其系数

3. 字段详细说明

3.1 反应物/生成物数组
- 格式：[[系数,"化学式"], ...]
- 示例：[[2,"H2"], [1,"O2"]] 表示 2H2 + O2
- 化学式使用HTML下标标签<sub>表示下标
- 系数为整数或分数形式

3.2 反应条件
- 描述反应所需的条件，如：
  - "点燃"、"加热"、"高温"、"常温常压"
  - "催化剂"、"光照"、"通电"等
- 特殊条件会详细说明

4. 化学式表示规范
- 元素符号首字母大写
- 下标使用<sub>标签，如CO<sub>2</sub>
- 离子团用括号括起，如Ca(OH)<sub>2</sub>
- 有机化合物用分子式表示，如CH<sub>4</sub>

5. 反应分类
反应按主题分组，用注释标记：
- 0-x：碳及其化合物的反应
- 1-x：硫、磷等非金属的反应
- 2-x：高锰酸钾、过氧化氢等氧化还原反应
- 3-x：金属氧化物与还原反应

6. 注意事项
- 每一个反应应配上注释！！！！用json5的//
- 所有系数已配平
- 反应条件不同可能导致不同产物
- 可逆反应会特别标注
- 工业常用反应会注明


9. 示例
{
    "反应物": [[2,"H2"], [1,"O2"]],
    "反应条件": "点燃",
    "生成物": [[2,"H2O"]]
}
表示：2H2 + O2 →(点燃) 2H2O