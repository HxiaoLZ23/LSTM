   M S B A M L   `   `   `  ����   LSTM.UI    LSTM.UI.Views.LoginWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$%  LSTM情感分析系统 - 登录��$	��500��$	��400�� � WindowStartupLocation$ CenterScreen=� � 
ResizeMode$ NoResize=�   MaterialDesignPaper#���     MaterialDesignBody#���    MaterialDesignFont#���  .���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�#  MaterialDesignThemes.Wpf.Card  $��0��$��20,20,20,10q�+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�.����  �� Orientation$ Vertical=�$��20q�.+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Brain=�$��64��$��64��$��Center=�  PrimaryHueMidBrush#���  �� $��LSTM情感分析系统��$��24(�$
��Bold"�$��Center=�$��0,10,0,0q��� $-��'基于机器学习的文本情感识别��$��14(�$��Center=� J� Opacity$	 0.7Q�$��0,5,0,0q�  $��1��$��20,10,20,10q�$ Depth2=�.���� $��30q�.+��� $��用户登录��$��20(�$��SemiBold"�$��Center=�$��0,0,0,30q��� -    ��Name$ UsernameTextBox��)  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 	用户名��
   
IsFloating$

 True�� ! MaterialDesignOutlinedTextBox#��[ $��0,0,0,20q����� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�G� -    G�Name$ PasswordBox��$	 密码��$

 True�� % !MaterialDesignOutlinedPasswordBox#��[ $��0,0,0,20q��� 
���  ��$��12(�$��0,0,0,10q����� *ErrorMessage+y��� *ErrorMessage+  StringToVisibilityConverter �� 	Converter# [ �� $��登录��  MaterialDesignRaisedButton#��[  �� 	IsDefault$
 True��$��40��$��0,10,0,0q����� *LoginCommand+�  � IsIndeterminate$
 True��$��0,10,0,0q�y��� *	IsLoading+   BooleanToVisibilityConverter# [ �� $ 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� $��还没有账户？��$��Center=��� $��立即注册�� 	 MaterialDesignFlatButton#��[	 $��5,0,0,0q����� *RegisterCommand+�� $��2��$ 
Horizontal=�$��Center=�$��20q�.+��� $��退出��  
 MaterialDesignOutlinedButton#��[
 $��80������ *ExitCommand+