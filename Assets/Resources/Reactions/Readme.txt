reactions: 一个字典列表，描述本关可能涉及的所有反应。
对于每一个反应，由一个字典(Dictionary，或哈希表)来描述：
    reactants: 一个(string, int)字典，描述所有的反应物。键是每一个反应物的id字符串，值是该反应物的化学计量数。
    condition: 一个非负整数列表，描述反应条件。数字具体指的反应条件见Assets/Resources/Levels/Readme.txt。
    products: 一个非负整数列表，描述所有的生成物。列表按 生成物id、生成物的化学计量数…… 为序。