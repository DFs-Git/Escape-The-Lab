整个JSON文件可以看作一个字典(哈希表)。
键(key): 由各反应物的ID组成的字符串。写成这样是方便查询。
值(value): 一个字典列表，描述了通过反应物的不同配比导致的不同反应。
对于上述每一个字典:
    reactants: 一个字典，描述了各反应物的份数关系。具体而言，键指反应物的ID字符串，值指这种反应物应该有几份。
    condition: 一个正整数，描述了反应发生的条件。正整数->反应条件的映射见Resources/Levels/Readme.txt。
    products: 一个二维列表，对于其中的每一个列表，描述了反应的产物。具体见Resources/Levels/Readme.txt。