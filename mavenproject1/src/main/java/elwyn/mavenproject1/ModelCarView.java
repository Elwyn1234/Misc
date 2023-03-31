package elwyn.mavenproject1;

import java.awt.Color;
import java.awt.FlowLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.BorderFactory;
import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;

public class ModelCarView {
    List<ModelCar> modelCars = new ArrayList<ModelCar>();
    JPanel displayPanel;
    Connection conn;
    
    public ModelCarView(JPanel displayPanel) {
        try {
            Class.forName("org.sqlite.JDBC");
            conn = DriverManager.getConnection("jdbc:sqlite:assets/mcec.db");
        } catch (Exception ex) {
            Logger.getLogger(ModelCarView.class.getName()).log(Level.SEVERE, null, ex);
        }
        this.displayPanel = displayPanel;
        loadModelCars();
        initComponents();
    }
    
    public void loadModelCars() {
        try {
            String sql = "SELECT rowid, * from modelCars";
            PreparedStatement pStatement = conn.prepareStatement(sql);
            ResultSet rs = pStatement.executeQuery();
            
            while (rs.next()) {
                ModelCar modelCar = new ModelCar();
                modelCar.id = rs.getInt("rowid");
                modelCar.name = rs.getString("name");
                modelCar.description = rs.getString("description");
                modelCar.category = rs.getString("category");
                modelCar.price = rs.getString("price");
                modelCar.imageFilePath = rs.getString("imageFilePath");
                modelCars.add(modelCar);
            }
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
        filterModelCars();
    }
    
    public void filterModelCars() {}
    
    public void addExtraLabels(JPanel panel, ModelCar modelCar) {}
    
    public void initComponents() {
//        JButton createButton = new JButton();
//        createButton.addActionListener(new ActionListener() {
//            @Override
//            public void actionPerformed(ActionEvent e) {
//                System.out.println("hi from button");
//            }
//        });
//        displayPanel.add(createButton);
        
        for (var modelCar : modelCars) {
            JPanel item = new JPanel();
            item.setSize(1028, 330);
            item.setBorder(BorderFactory.createLineBorder(Color.black));
            item.setLayout(new FlowLayout(FlowLayout.CENTER, 40, 10));
            
            JPanel leftFields = new JPanel();
            leftFields.setSize(300, 330);
            leftFields.setLayout(new BoxLayout(leftFields, BoxLayout.Y_AXIS));
            
            JPanel rightFields = new JPanel();
            rightFields.setSize(600, 330);
            rightFields.setLayout(new BoxLayout(rightFields, BoxLayout.Y_AXIS));

            
            
            leftFields.add(new JLabel("Name"));
            leftFields.add(new JLabel(modelCar.name));
            
            JLabel categoryLabel = new JLabel("Category");
            categoryLabel.setBorder(BorderFactory.createEmptyBorder(30, 0, 0, 0));
            leftFields.add(categoryLabel);
            leftFields.add(new JLabel(modelCar.category));
            
            JLabel priceLabel = new JLabel("Price");
            priceLabel.setBorder(BorderFactory.createEmptyBorder(30, 0, 0, 0));
            leftFields.add(priceLabel);
            leftFields.add(new JLabel(modelCar.price));
            
            addExtraLabels(leftFields, modelCar);
            
            JTextArea descriptionTextArea = new JTextArea(modelCar.description, 10, 40);
            descriptionTextArea.setEditable(false);
            descriptionTextArea.setLineWrap(true);
            rightFields.add(new JLabel("Description"));
            rightFields.add(descriptionTextArea);
            
            
            
            item.add(leftFields);
            item.add(rightFields);
            displayPanel.add(item);
        }        
    }
}
