   M S B A M L   `   `   `  ����   LSTM.UI    LSTM.UI.Views.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$  LSTM情感分析系统��$	��700��$
��1000�� � WindowStartupLocation$ CenterScreen=�   MaterialDesignPaper#���     MaterialDesignBody#���    MaterialDesignFont#���  .���  � RowDefinitions �� $
��Auto ��� $��* �.+�#  MaterialDesignThemes.Wpf.Card  $��0��+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�.��� $��20,10q� � ColumnDefinitions �� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�.+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Brain=�$��32��$��32��  PrimaryHueMidBrush#���  $��0,0,10,0q��� $��LSTM情感分析系统��$��18(�$��SemiBold"�$��Center=��� $��14(�$��Center=�$��20,0,0,0q� J� Opacity$	 0.7Q����� *CurrentUser.Username+ �� StringFormat$ 欢迎，{0}！���� $��1��$��退出登录��   MaterialDesignOutlinedButton#��[ ���� *LogoutCommand+�� $��1�� " MaterialDesignFilledTabControl#��[ $��20q�	 �� SelectedIndex	 �� *SelectedTabIndex+.@��� $��文本分析������ .R��� $ 
Horizontal=�.+�  $ 
TextSearch=�$��16��$��16��$��0,0,5,0q��� $��文本分析��.��� $��20q� �� $
��Auto ��� $	��200 ��� $
��Auto ��� $��* �.+��� $��0��$$��请输入要分析的文本：��$��16(�$��SemiBold"�$��0,0,0,10q�  $��1��$ Depth1=�.���� 
 �� AcceptsReturn$

 True�� �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$3 -在此输入要分析情感的文本内容...�� ! MaterialDesignOutlinedTextBox#��[ $��10q����� *	InputText+ �� UpdateSourceTrigger$ PropertyChanged=��� $��2��$ 
Horizontal=�$��Center=�$��	0,20,0,20q�.+��� $��开始分析��  MaterialDesignRaisedButton#��[ $	��120��$��36��$��0,0,10,0q����� *AnalyzeCommand+�� $��清空��#��[ $��80��$��36������ *ClearCommand+  $��3��$ Depth2=�y��� *CurrentResult+  StringToVisibilityConverter �� 	Converter# [ .���� $��20q�.+��� $��分析结果��$��16(�$��SemiBold"�$��0,0,0,15q��  �� $
��Auto ��� $��* � �� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��情感类型：��$��SemiBold"�$��	0,0,10,10q��� $��0��$��1��$��16(�$��0,0,0,10q����� *CurrentResult.SentimentLabel+�� $��1��$��0��$��置信度：��$��SemiBold"�$��	0,0,10,10q��� $��1��$��1��$ 
Horizontal=�$��0,0,0,10q�.+��  � Maximum$ 1Q�$	��200��$��8��$��0,0,10,0q� � Value �� *CurrentResult.Confidence+�� $��Center=����� *CurrentResult.Confidence+$ {0:P1}���� $��2��$��0��$��分析时间：��$��SemiBold"�$��0,0,10,0q��� $��2��$��1������ *CurrentResult.AnalyzedAt+$ {0:yyyy-MM-dd HH:mm:ss}��� $��3�� � IsIndeterminate$
 True��$��Center=�$��4��y��� *IsAnalyzing+  	 BooleanToVisibilityConverter# [	 �� $��历史记录������ .R��� $ 
Horizontal=�.+�  $ History=�$��16��$��16��$��0,0,5,0q��� $��历史记录��.��� $��20q� �� $
��Auto ��� $��* �.+��� $��0��$ 
Horizontal=�$��0,0,0,20q�.+��� $��刷新��#��[ $��0,0,10,0q����� *LoadHistoryCommand+�� $��Center=�$��10,0,0,0q�$	 0.7Q����� *AnalysisHistory.Count+$ 共 {0} 条记录��  $��1��$ Depth1=�.��&   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��   
IsReadOnly$
 True��   SelectionMode$ Single=�   GridLinesVisibility$ 
Horizontal=�   HeadersVisibility$ Column=����� *AnalysisHistory+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 文本内容��   Width5  /System.Windows.Controls.DataGridLengthConverter$ *    MaxWidth$	 300Q�1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *Text+ 	  ElementStyle ��  �� 
TargetType" ��.���  �� Property � ��  �� Value$
  Wrap=���  � ��$  60��  $ 情感类型��$	 100  �� *SentimentLabel+  $ 	置信度��$ 80  �� *
Confidence+$ {0:P1}��  $ 分析时间��$	 120  �� *
AnalyzedAt+$ {0:MM-dd HH:mm}���� $��模型训练������ .R��� $ 
Horizontal=�.+�  $ Brain=�$��16��$��16��$��0,0,5,0q��� $��模型训练��.��� $��20q� �� $��* ��� $��* �.+�  $��0��$ Depth2=�$��0,0,10,0q�.���� $��20q�.+��� $��数据集管理��$��16(�$��SemiBold"�$��0,0,0,20q�9� $��创建新数据集��$��0,0,0,20q�.���� $��10q�.+��� $ 数据集名称��!   
IsFloating$
! True��#��[ $��0,0,0,15q����� *NewDatasetName+$ PropertyChanged=��� $ 描述（可选）��$
! True��#��[ $��0,0,0,15q����� *NewDatasetDescription+$ PropertyChanged=��� $��创建数据集��#��[ $��Right=����� *CreateDatasetCommand+�� $��现有数据集：��$��SemiBold"�$��0,0,0,10q��� $	��200������ *
Datasets+" �� SelectedItem" �� *SelectedDataset+���� .R��� .+��� $��SemiBold"����� *Name+�� $	 0.7Q�$��12(����� *Description+�� $��11(�$	 0.5Q����� *	CreatedAt+$  创建于：{0:yyyy-MM-dd}���� $��刷新数据集��#��[ $��0,10,0,0q����� *RefreshDatasetsCommand+  $��1��$ Depth2=�$��10,0,0,0q�.���� $��20q�.+��� $��模型训练��$��16(�$��SemiBold"�$��0,0,0,20q�9� $��训练新模型��$��0,0,0,20q�.���� $��10q�.+��� $ 模型名称��$
! True��#��[ $��0,0,0,15q����� *NewModelName+$ PropertyChanged=��� $��0,0,0,10q�$��12(����� *SelectedDataset.Name+$ 选中数据集：{0}���� $��开始训练��#��[ $��Right=����� *TrainModelCommand+� $
 True��$��0,10,0,0q�y��� *
IsTraining+# [	 �� $��已训练模型：��$��SemiBold"�$��0,0,0,10q��� $	��200������ *Models+" �� *SelectedModel+���� .R��� .+��� $��SemiBold"����� *Name+�� $��12(�$	 0.7Q����� *
Accuracy+$ 准确率：{0:P2}���� $��11(�$	 0.5Q����� *	TrainedAt+$&  训练于：{0:yyyy-MM-dd HH:mm}���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��加载模型��#��[ $��0,0,10,0q����� *LoadModelCommand+�� $��刷新模型��#��[ ���� *RefreshModelsCommand+