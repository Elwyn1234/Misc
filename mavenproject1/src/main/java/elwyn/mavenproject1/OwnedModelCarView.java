package elwyn.mavenproject1;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;
import javax.swing.BorderFactory;
import javax.swing.JLabel;
import javax.swing.JPanel;

public class OwnedModelCarView extends ModelCarView {
    public OwnedModelCarView(JPanel panel) {
        super(panel);
    }
    
    @Override
    public void loadModelCars() {
        super.loadModelCars();
        try {
            String sql = "SELECT * FROM ownedModelCars WHERE username='user1'";
            PreparedStatement pStatement = conn.prepareStatement(sql);
            ResultSet rs = pStatement.executeQuery();
            
            while (rs.next()) {
                ModelCar modelCar = new ModelCar();
                for (int i = 0; i < modelCars.size(); i++) {
                    if (modelCars.get(i).id == rs.getInt("modelCarId"))
                        modelCar = modelCars.get(i);
                }
                modelCar.dateCollected = rs.getString("dateCollected");
            }
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }
    
    @Override
    public void filterModelCars() {
        try {
            String sql = "SELECT * from ownedModelCars WHERE username='user1'";
            PreparedStatement pStatement = conn.prepareStatement(sql);
            ResultSet rs = pStatement.executeQuery();

            List<ModelCar> filteredModelCars = new ArrayList<ModelCar>();
            while (rs.next()) {
                for (int i = 0; i < modelCars.size(); i++) {
                    if (modelCars.get(i).id == rs.getInt("modelCarId")) {
                        filteredModelCars.add(modelCars.get(i));
                    }
                }
            }
            modelCars = filteredModelCars;
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }
    
    @Override
    public void addExtraLabels(JPanel panel, ModelCar modelCar) {
        JLabel dateCollectedLabel =dateCollectedLabel new JLabel("Date Collected");
        dateCollectedLabel.setBorder(BorderFactory.createEmptyBorder(30, 0, 0, 0));
        panel.add(dateCollectedLabel);
        panel.add(new JLabel(modelCar.dateCollected));
    }
}
