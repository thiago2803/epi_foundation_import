@echo off 
cls 
echo ###################################################################### 
echo #           Rebuid the current application from default              # 
echo ###################################################################### 
echo #                                                                    # 
echo #       NOTE: This will **DROP** the existing DB                     # 
echo #             and resetup so use with caution!!                      # 
echo #                                                                    # 
echo #       Crtl+C NOW if you are unsure!                                # 
echo #                                                                    # 
echo ###################################################################### 
pause 
setup foundationimport "foundationimport" "foundationimport-cm" "" THIAGO-DELL\SQLEXPRESS -E 
