   M S B A M L   `   `   `  ����  LSTM.UI    LSTM.UI.Views.LoginWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$%  LSTM情感分析系统 - 登录��5   	   $	��800��5   	   $	��650��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5	   	      MaterialDesignPaper#���   5
   	     MaterialDesignBody#���  5   	     MaterialDesignFont#���  5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��0��6   $��20,20,20,10q�6+   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�6@   .��5      ��  �� Orientation$ Vertical=�6   $��20q�60   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Brain=�6*   $��64��5   (   $��64��5   (   $��Center=�5   (     PrimaryHueMidBrush#���  5   (   �� 5      $��LSTM情感分析系统��6   $��24(�5      $
��Bold"�5      $��Center=�5       $��0,10,0,0q�5!      �� 5"      $-��'基于机器学习的文本情感识别��6   $��14(�5#      $��Center=�5$       J� Opacity$	 0.7Q�5%      $��0,5,0,0q�5&      5'      5(        5+   
   $��1��6   $��20,10,20,10q�6+   $ Depth2=�6@   .��5,      �� $��30q�6   .+�5-      �� $��用户登录��6   $��20(�5.      $��SemiBold"�5/      $��Center=�50      $��0,0,0,30q�51      �� 54      -   5         ��Name$ UsernameTextBox��54      )  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 	用户名��56      
   
IsFloating$

 True��57       ! MaterialDesignOutlinedTextBox#��[ 58      $��0,0,0,20q�59      ��55      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�59      G� 5<      -   5         G�Name$ PasswordBox��5<      $	 密码��5=      $

 True��5>       % !MaterialDesignOutlinedPasswordBox#��[ 5?      $��0,0,0,20q�5@      �� 5C      
���  ��$��12(�5E      $��0,0,0,10q�5F      ��5C      �� *ErrorMessage+y�5G      �� *ErrorMessage+  StringToVisibilityConverter �� 	Converter# [ �� 5J      $��登录��6     MaterialDesignRaisedButton#��[ 5L       �� 	IsDefault$
 True��5M      $��40��5N      $��0,10,0,0q�5O      ��5K      �� *LoginCommand+5O      � 5R       � IsIndeterminate$
 True��6   $��0,10,0,0q�5T      y�5S      �� *	IsLoading+   BooleanToVisibilityConverter# [ 5T      �� 5W      $ 
Horizontal=�6   $��Center=�5X      $��0,20,0,0q�5Y      .+�5Z      �� $��还没有账户？��6    $��Center=�5[      �� 5\      $��立即注册��6    	 MaterialDesignFlatButton#��[	 5^      $��5,0,0,0q�5_      ��5]      �� *RegisterCommand+5_      5`      5a      5b      �� 5e   
   $��2��6   $ 
Horizontal=�5f      $��Center=�5g      $��20q�5h      .+�5i      �� $��退出��6     
 MaterialDesignOutlinedButton#��[
 5k      $��80��5l      ��5j      �� *ExitCommand+5l      5m      5n      5o      